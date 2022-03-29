using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Esprima;
using Esprima.Ast;
using Jint.DebugAdapter.BreakPoints;
using Jint.Native;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    internal class SynchronizingDebugger
    {
        private abstract class BaseMessage
        {
            public static readonly ContinueMessage Continue = new();
            public bool ShouldContinue { get; protected set; }

            public abstract void Invoke();
        }

        private class ContinueMessage : BaseMessage
        {
            public ContinueMessage()
            {
                ShouldContinue = true;
            }

            public override void Invoke()
            {
                // Do nothing
            }
        }

        private class Message<T> : BaseMessage
        {
            private readonly Func<T> action;
            private readonly TaskCompletionSource<T> tcs = new();

            public Task<T> Result => tcs.Task;

            public Message(Func<T> action)
            {
                this.action = action;
            }

            public override void Invoke()
            {
                try
                {
                    var result = action();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
        }

        private class Message : BaseMessage
        {
            private readonly Action action;

            private readonly TaskCompletionSource tcs = new();

            public Task Result => tcs.Task;

            public Message(Action action)
            {
                this.action = action;
            }

            public override void Invoke()
            {
                try
                {
                    action();
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
        }

        public delegate void DebugLogMessageEventHandler(string message, DebugInformation info);
        public delegate void DebugPauseEventHandler(PauseReason reason, DebugInformation info);
        public delegate void DebugEventHandler();
        public delegate void DebugExceptionEventHandler(Exception ex);

        private enum DebuggerState
        {
            WaitingForClient,
            WaitingForUI,
            Entering,
            Running,
            Pausing,
            Stepping,
            Terminating,
            Terminated
        }

        private readonly Dictionary<string, ScriptInfo> scriptInfoBySourceId = new();
        private readonly Engine engine;
        private readonly CancellationTokenSource cts = new();
        private readonly Channel<BaseMessage> channel = Channel.CreateUnbounded<BaseMessage>(new UnboundedChannelOptions { SingleReader = true, SingleWriter = false });
        private readonly int engineThreadId;
        private StepMode nextStep;
        private DebuggerState state;
        private bool pauseOnEntry;

        public bool IsAttached { get; private set; }

        public event DebugLogMessageEventHandler LogPoint;
        public event DebugPauseEventHandler Paused;
        public event DebugEventHandler Resumed;
        public event DebugEventHandler Cancelled;
        public event DebugEventHandler Done;
        public event DebugExceptionEventHandler Error;

        public SynchronizingDebugger(Engine engine)
        {
            this.engine = engine;
            engineThreadId = Environment.CurrentManagedThreadId;
        }

        public async Task<Location?> GetCurrentLocationAsync()
        {
            return await InvokeAsync(() => engine?.DebugHandler.CurrentLocation);
        }

        public ScriptInfo GetScriptInfo(string id)
        {
            if (!scriptInfoBySourceId.TryGetValue(id, out var info))
            {
                throw new DebuggerException($"Requested source '{id}' not loaded.");
            }
            return info;
        }

        public async Task AttachAsync(bool pause)
        {
            Post(() =>
            {
                Attach();
                if (pause)
                {
                    Pause();
                }
            });
        }

        public async Task LaunchAsync(Action action, bool debug, bool pauseOnEntry, bool attach, bool waitForUI)
        {
            this.pauseOnEntry = pauseOnEntry;

            var launchCompleted = new TaskCompletionSource();
            void HandleScriptReady(object sender, ScriptInformation info)
            {
                EnsureOnEngineThread();
                engine.DebugHandler.ScriptReady -= HandleScriptReady;
                launchCompleted.SetResult();

                // This will be called on the engine thread, hence we can pause it and the LaunchAsync method
                // (which is not on the engine thread) will still return
                if (waitForUI)
                {
                    WaitForUI();
                }
            }

            Post(() =>
            {
                if (debug)
                {
                    AddEventHandlers();
                    if (attach)
                    {
                        Attach();
                    }
                }
                try
                {
                    engine.DebugHandler.ScriptReady += HandleScriptReady;
                    action();
                    OnDone();
                }
                catch (Exception ex)
                {
                    if (ex is OperationCanceledException)
                    {
                        OnCancelled();
                    }
                    else
                    {
                        OnError(ex);
                        launchCompleted.SetException(ex);
                    }
                }
                finally
                {
                    RemoveEventHandlers();
                    // We detach regardless of whether we attached earlier - might have attached during script
                    // execution.
                    Detach();
                }
            });

            await launchCompleted.Task;
        }

        public async Task<JsValue> EvaluateAsync(string expression)
        {
            return await InvokeAsync(() => engine.DebugHandler.Evaluate(expression));
        }

        public async Task<JsValue> EvaluateAsync(Script expression)
        {
            return await InvokeAsync(() => engine.DebugHandler.Evaluate(expression));
        }

        /// <summary>
        /// Terminates script execution
        /// </summary>
        public void Terminate()
        {
            cts.Cancel();
            state = DebuggerState.Terminating;
            Resume();
        }

        public void StepOver()
        {
            nextStep = StepMode.Over;
            Resume();
        }

        public void StepInto()
        {
            nextStep = StepMode.Into;
            Resume();
        }

        public void StepOut()
        {
            nextStep = StepMode.Out;
            Resume();
        }

        public void Run()
        {
            state = DebuggerState.Running;
            nextStep = StepMode.None;
            Resume();
        }

        public void Pause()
        {
            state = DebuggerState.Pausing;
            nextStep = StepMode.Into;
        }

        public void Disconnect()
        {
            Detach();
            // Make sure we're not paused
            Resume();
        }

        public async Task ClearBreakPointsAsync()
        {
            await InvokeAsync(() => engine.DebugHandler.BreakPoints.Clear());
        }

        public async Task<Position> SetBreakPointAsync(string sourceId, Position position, string condition = null, string hitCondition = null, string logMessage = null)
        {
            var info = GetScriptInfo(sourceId);
            position = info.FindNearestBreakPointPosition(position);

            await InvokeAsync(() =>
            {
                engine.DebugHandler.BreakPoints.Set(new ExtendedBreakPoint(sourceId, position.Line, position.Column, condition, hitCondition, logMessage));
            });
            
            return position;
        }

        public void Attach()
        {
            if (IsAttached)
            {
                throw new InvalidOperationException($"Attempt to attach debugger when already attached.");
            }
            IsAttached = true;
        }

        public void Detach()
        {
            if (!IsAttached)
            {
                return;
            }
            IsAttached = false;
        }

        public void WaitForClient()
        {
            state = DebuggerState.WaitingForClient;
            Wait();
        }

        public void WaitForUI()
        {
            state = DebuggerState.WaitingForUI;
            Wait();
        }

        public void NotifyUIReady()
        {
            state = DebuggerState.Entering;
            Resume();
        }

        protected virtual void OnDone()
        {
            Done?.Invoke();
        }

        protected virtual void OnCancelled()
        {
            Cancelled?.Invoke();
        }

        protected virtual void OnError(Exception ex)
        {
            Error?.Invoke(ex);
        }

        protected virtual void OnLogPoint(string message, DebugInformation info)
        {
            LogPoint?.Invoke(message, info);
        }

        protected virtual void OnPaused(PauseReason reason, DebugInformation info)
        {
            Paused?.Invoke(reason, info);
        }

        protected virtual void OnResumed()
        {
            Resumed?.Invoke();
        }

        private void AddEventHandlers()
        {
            engine.DebugHandler.ScriptReady += DebugHandler_ScriptReady;
            engine.DebugHandler.Break += DebugHandler_Break;
            engine.DebugHandler.Step += DebugHandler_Step;
            engine.DebugHandler.Skip += DebugHandler_Skip;
        }

        private void RemoveEventHandlers()
        {
            engine.DebugHandler.ScriptReady -= DebugHandler_ScriptReady;
            engine.DebugHandler.Break -= DebugHandler_Break;
            engine.DebugHandler.Step -= DebugHandler_Step;
            engine.DebugHandler.Skip -= DebugHandler_Skip;
        }

        private void Resume()
        {
            channel.Writer.TryWrite(new ContinueMessage());
        }

        private void DebugHandler_ScriptReady(object sender, ScriptInformation e)
        {
            RegisterScriptInfo(e.Ast.Location.Source, e.Ast);
        }

        private StepMode DebugHandler_Step(object sender, DebugInformation e)
        {
            cts.Token.ThrowIfCancellationRequested();

            // This (and the same call in Break and Step) is purely to handle attach request messages (which would be
            // ignored, because no messages are handled when not attached). Consider making IsAttached shared between
            // threads so it can simply be set directly.
            HandleMessages();
            if (!IsAttached)
            {
                return StepMode.None;
            }

            HandleBreakPoint(e);

            switch (state)
            {
                case DebuggerState.WaitingForClient:
                case DebuggerState.WaitingForUI:
                    throw new InvalidOperationException("Debugger should not be stepping while waiting for client or UI");

                case DebuggerState.Entering:
                    if (!pauseOnEntry)
                    {
                        state = DebuggerState.Running;
                        return StepMode.None;
                    }
                    state = DebuggerState.Stepping;
                    return OnPause(PauseReason.Entry, e);

                case DebuggerState.Running:
                    return StepMode.None;

                case DebuggerState.Pausing:
                    state = DebuggerState.Stepping;
                    return OnPause(PauseReason.Pause, e);

                case DebuggerState.Stepping:
                    return OnPause(PauseReason.Step, e);

                case DebuggerState.Terminating:
                    throw new InvalidOperationException("Debugger should not be stepping while terminating");

                default:
                    throw new NotImplementedException($"Debugger state handling for {state} not implemented.");
            }

        }

        private StepMode DebugHandler_Break(object sender, DebugInformation e)
        {
            cts.Token.ThrowIfCancellationRequested();

            HandleMessages();
            if (!IsAttached)
            {
                return StepMode.None;
            }

            bool breakPointShouldBreak = HandleBreakPoint(e);

            switch (e.PauseType)
            {
                case PauseType.DebuggerStatement:
                    state = DebuggerState.Stepping;
                    return OnPause(PauseReason.DebuggerStatement, e);

                case PauseType.Break:
                    if (breakPointShouldBreak)
                    {
                        state = DebuggerState.Stepping;
                        return OnPause(PauseReason.BreakPoint, e);
                    }
                    break;
            }

            // Break is only called when we're not stepping - so since we didn't pause, keep running:
            return StepMode.None;
        }


        private StepMode DebugHandler_Skip(object sender, DebugInformation e)
        {
            cts.Token.ThrowIfCancellationRequested();

            HandleMessages();
            if (!IsAttached)
            {
                return StepMode.None;
            }

            // Skip allows us to change the stepmode (i.e. pause) when we're in StepMode.None (i.e. running)
            return nextStep;
        }

        private bool HandleBreakPoint(DebugInformation info)
        {
            if (info.BreakPoint == null)
            {
                return false;
            }
            if (info.BreakPoint is ExtendedBreakPoint breakPoint)
            {
                // If breakpoint has a hit condition, evaluate it
                if (breakPoint.HitCondition != null)
                {
                    breakPoint.HitCount++;
                    if (!breakPoint.HitCondition(breakPoint.HitCount))
                    {
                        // Don't break if the hit condition wasn't met
                        return false;
                    }
                }

                // If this is a logpoint rather than a breakpoint, log message and don't break
                if (breakPoint.LogMessage != null)
                {
                    // We're on the engine thread (it called us), so we're free to use Evaluate directly:
                    var message = engine.DebugHandler.Evaluate(breakPoint.LogMessage);
                    OnLogPoint(message.AsString(), info);
                    return false;
                }
            }

            // Allow breakpoint to break
            return true;
        }

        private StepMode OnPause(PauseReason reason, DebugInformation e)
        {
            OnPaused(reason, e);

            Wait();

            OnResumed();

            return nextStep;
        }

        private void RegisterScriptInfo(string id, Program ast)
        {
            // This may be called multiple times with the same ID (on e.g. restart)
            scriptInfoBySourceId[id] = new ScriptInfo(ast);
        }

        private void Wait()
        {
            EnsureOnEngineThread();
            while (true)
            {
                // TODO: Find a way to consume in single thread without constant looping
                if (_ConsumeMessages())
                {
                    break;
                }
            }
        }

        private void HandleMessages()
        {
            EnsureOnEngineThread();
            _ConsumeMessages();
        }

        private bool _ConsumeMessages()
        {
            // TODO: Find a way to consume in single thread without constant looping
            while (channel.Reader.TryRead(out var message))
            {
                if (message.ShouldContinue)
                {
                    return true;
                }
                message.Invoke();
            }
            return false;
        }

        private void EnsureOnEngineThread()
        {
            Debug.Assert(Environment.CurrentManagedThreadId == engineThreadId, "Not on engine thread");
        }

        private async Task<T> InvokeAsync<T>(Func<T> action)
        {
            var message = new Message<T>(action);
            channel.Writer.TryWrite(message);
            return await message.Result;
        }

        private async Task InvokeAsync(Action action)
        {
            var message = new Message(action);
            channel.Writer.TryWrite(message);
            await message.Result;
        }

        private void Post(Action action)
        {
            var message = new Message(action);
            channel.Writer.TryWrite(message);
        }
    }
}
