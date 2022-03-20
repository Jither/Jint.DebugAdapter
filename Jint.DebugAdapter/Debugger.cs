using Esprima;
using Esprima.Ast;
using Jint.DebugAdapter.Breakpoints;
using Jint.Native;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public class Debugger
    {
        public delegate void DebugLogMessageEventHandler(string message, DebugInformation info);
        public delegate void DebugPauseEventHandler(PauseReason reason, DebugInformation info);
        public delegate void DebugEventHandler();
        public delegate void DebugExceptionEventHandler(Exception ex);

        private enum DebuggerState
        {
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
        private CancellationTokenSource cts;
        private StepMode nextStep;
        private DebuggerState state;

        public bool PauseOnEntry { get; set; }
        public bool IsAttached { get; private set; }
        public Location? CurrentLocation => engine?.DebugHandler.CurrentLocation;

        public IPauseHandler PauseHandler { get; set; } = new ResetEventPauseHandler();

        public event DebugLogMessageEventHandler LogPoint;
        public event DebugPauseEventHandler Paused;
        public event DebugEventHandler Resumed;
        public event DebugEventHandler Cancelled;
        public event DebugEventHandler Done;
        public event DebugExceptionEventHandler Error;

        public Debugger(Engine engine)
        {
            this.engine = engine;
        }

        public void Execute(string sourceId, string source, bool debug)
        {
            cts = new CancellationTokenSource();
            var ast = PrepareScript(sourceId, source);
            Task.Run(() =>
            {
                if (debug)
                {
                    Attach();
                }
                try
                {
                    // Pause the engine thread, to wait for the debugger UI
                    state = DebuggerState.WaitingForUI;
                    PauseHandler.Pause();

                    engine.Execute(ast);
                    OnDone();
                }
                finally
                {
                    Detach();
                    state = DebuggerState.Terminated;
                }
            }, cts.Token)
            .ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    OnCancelled();
                }
                if (t.IsFaulted)
                {
                    // TODO: Better handling
                    OnError(t.Exception.InnerExceptions[0]);
                }
            });
        }

        public ScriptInfo GetScriptInfo(string id)
        {
            if (!scriptInfoBySourceId.TryGetValue(id, out var info))
            {
                throw new DebuggerException($"Requested source '{id}' not loaded.");
            }
            return info;
        }

        public JsValue Evaluate(string expression)
        {
            return engine.DebugHandler.Evaluate(expression);
        }

        public JsValue Evaluate(Script expression)
        {
            return engine.DebugHandler.Evaluate(expression);
        }

        /// <summary>
        /// Terminates script execution
        /// </summary>
        public void Terminate()
        {
            cts.Cancel();
            state = DebuggerState.Terminating;
            PauseHandler.Resume();
        }

        public void StepOver()
        {
            nextStep = StepMode.Over;
            PauseHandler.Resume();
        }

        public void StepInto()
        {
            nextStep = StepMode.Into;
            PauseHandler.Resume();
        }

        public void StepOut()
        {
            nextStep = StepMode.Out;
            PauseHandler.Resume();
        }

        public void Run()
        {
            state = DebuggerState.Running;
            nextStep = StepMode.None;
            PauseHandler.Resume();
        }

        public void Pause()
        {
            state = DebuggerState.Pausing;
        }

        public void Disconnect()
        {
            Detach();
            // Make sure we're not paused
            PauseHandler.Resume();
        }

        public void ClearBreakpoints()
        {
            engine.DebugHandler.BreakPoints.Clear();
        }

        public Position SetBreakpoint(string sourceId, Position position, string condition = null, string hitCondition = null, string logMessage = null)
        {
            var info = GetScriptInfo(sourceId);
            position = info.FindNearestBreakpointPosition(position);

            engine.DebugHandler.BreakPoints.Set(new ExtendedBreakPoint(
                sourceId, position.Line, position.Column, condition, hitCondition, logMessage));
            return position;
        }

        public void NotifyUIReady()
        {
            state = DebuggerState.Entering;
            PauseHandler.Resume();
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

        private void Attach()
        {
            if (IsAttached)
            {
                throw new InvalidOperationException($"Attempt to attach debugger when already attached.");
            }
            IsAttached = true;
            engine.DebugHandler.Break += DebugHandler_Break;
            engine.DebugHandler.Step += DebugHandler_Step;
        }

        private void Detach()
        {
            if (!IsAttached)
            {
                return;
            }
            engine.DebugHandler.Break -= DebugHandler_Break;
            engine.DebugHandler.Step -= DebugHandler_Step;
            IsAttached = false;
        }

        private Script PrepareScript(string sourceId, string source)
        {
            var parser = new JavaScriptParser(source, new ParserOptions(sourceId) { Tokens = true, AdaptRegexp = true, Tolerant = true });
            var ast = parser.ParseScript();
            RegisterScriptInfo(sourceId, ast);
            return ast;
        }

        private void RegisterScriptInfo(string id, Script ast)
        {
            // This may be called multiple times with the same ID (on e.g. restart)
            scriptInfoBySourceId[id] = new ScriptInfo(ast);
        }

        private StepMode DebugHandler_Step(object sender, DebugInformation e)
        {
            cts.Token.ThrowIfCancellationRequested();

            if (!IsAttached)
            {
                return StepMode.None;
            }

            HandleBreakpoint(e);

            switch (state)
            {
                case DebuggerState.WaitingForUI:
                    throw new InvalidOperationException("Debugger should not be stepping while waiting for UI");

                case DebuggerState.Entering:
                    if (!PauseOnEntry)
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

            bool breakPointShouldBreak = HandleBreakpoint(e);

            switch (e.PauseType)
            {
                case PauseType.DebuggerStatement:
                    state = DebuggerState.Stepping;
                    return OnPause(PauseReason.DebuggerStatement, e);

                case PauseType.Break:
                    if (breakPointShouldBreak)
                    {
                        state = DebuggerState.Stepping;
                        return OnPause(PauseReason.Breakpoint, e);
                    }
                    break;
            }

            // Break is only called when we're not stepping - so since we didn't pause, keep running:
            return StepMode.None;
        }

        private bool HandleBreakpoint(DebugInformation info)
        {
            if (info.BreakPoint == null)
            {
                return false;
            }
            if (info.BreakPoint is ExtendedBreakPoint breakpoint)
            {
                // If breakpoint has a hit condition, evaluate it
                if (breakpoint.HitCondition != null)
                {
                    breakpoint.HitCount++;
                    if (!breakpoint.HitCondition(breakpoint.HitCount))
                    {
                        // Don't break if the hit condition wasn't met
                        return false;
                    }
                }

                // If this is a logpoint rather than a breakpoint, log message and don't break
                if (breakpoint.LogMessage != null)
                {
                    var message = Evaluate(breakpoint.LogMessage);
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

            PauseHandler.Pause();

            OnResumed();

            return nextStep;
        }
    }
}
