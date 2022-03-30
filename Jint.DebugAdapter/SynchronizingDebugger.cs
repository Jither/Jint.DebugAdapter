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
        private long _isAttached;

        public bool IsAttached
        {
            get => Interlocked.Read(ref _isAttached) == 1;
            private set
            {
                Interlocked.CompareExchange(ref _isAttached, value ? 1 : 0, value ? 0 : 1);
            }
        }

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

        public void Attach(bool pause)
        {
            if (pause)
            {
                Post(() => Pause());
            }
            InternalAttach();
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

                // This will be called on the engine thread, hence we can pause, and the LaunchAsync method
                // (which is *not* on the engine thread) will still return
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
                        InternalAttach();
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
                    InternalDetach();
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
            InternalDetach();
            // Make sure we're not paused - disconnection means execution should continue without the debugger.
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

        // Note: InternalAttach may (will) be called from multiple threads. Since synchronized messages aren't consumed
        // when the debugger isn't attached, we can't attach through a message. Hence, IsAttached is set up for thread
        // safe access.
        private void InternalAttach()
        {
            if (IsAttached)
            {
                throw new InvalidOperationException($"Attempt to attach debugger when already attached.");
            }
            IsAttached = true;
        }

        private void InternalDetach()
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
            // Wait until UI calls NotifyUIReady to indicate that it's ready for the script to execute.
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

            if (!IsAttached)
            {
                return StepMode.None;
            }

            // The Step handler may encounter breakpoints. Normal breakpoints don't need any handling while stepping
            // (that's the reason Break isn't called in the first place). However, we're dealing with hit count
            // conditions, which still need to increment the number of times the breakpoint has been passed. And
            // logpoints, which still need to log, even when stepping through the logpoint.
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

            if (!IsAttached)
            {
                return StepMode.None;
            }

            // Skip allows us to change the stepmode (i.e. pause) when we're in StepMode.None (i.e. running)
            return nextStep;
        }

        private bool HandleBreakPoint(DebugInformation info)
        {
            // Custom "extensions" to Jint's breakpoint functionality - hit (count) conditions and logpoints.
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

        /// <summary>
        /// Pauses execution, while still processing messages
        /// </summary>
        private void Wait()
        {
            while (true)
            {
                // We want to block here - that's how we pause script execution: by blocking the engine thread.
                channel.Reader.WaitToReadAsync(cts.Token).AsTask().Wait();

                if (ProcessMessages() == ProcessMessagesResult.ExecutionShouldContinue)
                {
                    // A message included a request to resume execution (e.g. continue/step)
                    break;
                }
            }
        }

        private enum ProcessMessagesResult
        {
            KeepWaiting,
            ExecutionShouldContinue
        }

        /// <summary>
        /// Processes all messages currently queued
        /// </summary>
        /// <returns>True if </returns>
        private ProcessMessagesResult ProcessMessages()
        {
            while (channel.Reader.TryRead(out var message))
            {
                if (message.ShouldContinue)
                {
                    return ProcessMessagesResult.ExecutionShouldContinue;
                }
                EnsureOnEngineThread();
                message.Invoke();
            }
            return ProcessMessagesResult.KeepWaiting;
        }

        [Conditional("DEBUG")]
        private void EnsureOnEngineThread()
        {
            Debug.Assert(Environment.CurrentManagedThreadId == engineThreadId, "Not on engine thread");
        }

        /// <summary>
        /// Sends an action (function, i.e. with result) to the message queue and awaits the result.
        /// </summary>
        private async Task<T> InvokeAsync<T>(Func<T> action)
        {
            var message = new Message<T>(action);
            channel.Writer.TryWrite(message);
            return await message.Result;
        }

        /// <summary>
        /// Sends an action to the message queue and awaits its completion.
        /// </summary>
        private async Task InvokeAsync(Action action)
        {
            var message = new Message(action);
            channel.Writer.TryWrite(message);
            await message.Result;
        }

        /// <summary>
        /// Posts an action to the message queue without waiting for it to be invoked.
        /// </summary>
        private void Post(Action action)
        {
            var message = new Message(action);
            channel.Writer.TryWrite(message);
        }
    }
}
