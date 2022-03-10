using System.Threading.Channels;
using Esprima;
using Esprima.Ast;
using Jint.Native;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public delegate void DebugPauseEventHandler(PauseReason reason, DebugInformation info);
    public delegate void DebugEventHandler();

    public class Debugger
    {
        private enum DebuggerState
        {
            WaitingForUI,
            Entering,
            Running,
            Pausing,
            Stepping,
            Terminating
        }

        private readonly Dictionary<string, ScriptInfo> scriptInfoBySourceId = new();
        private readonly Engine engine;
        private readonly ManualResetEvent waitForContinue = new(false);
        private readonly CancellationTokenSource cts = new();
        private StepMode nextStep;
        private DebuggerState state;

        public bool PauseOnEntry { get; set; }
        public bool IsAttached { get; private set; }
        public DebugInformation CurrentDebugInformation { get; private set; }
        public Engine Engine => engine;

        public event DebugPauseEventHandler Stopped;
        public event DebugEventHandler Continued;
        public event DebugEventHandler Cancelled;
        public event DebugEventHandler Done;

        public Debugger(Engine engine)
        {
            this.engine = engine;
        }

        public void Execute(string sourceId, string source, bool debug)
        {
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
                    PauseThread();

                    engine.Execute(ast);
                    Done?.Invoke();
                }
                finally
                {
                    Detach();
                }
            }, cts.Token).ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    Cancelled?.Invoke();
                }
                if (t.IsFaulted)
                {
                    throw t.Exception;
                }
            });
        }

        public ScriptInfo GetScriptInfo(string id)
        {
            return scriptInfoBySourceId.GetValueOrDefault(id);
        }

        public JsValue Evaluate(string expression)
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
            waitForContinue.Set();
        }

        public void StepOver()
        {
            nextStep = StepMode.Over;
            waitForContinue.Set();
        }

        public void StepInto()
        {
            nextStep = StepMode.Into;
            waitForContinue.Set();
        }

        public void StepOut()
        {
            nextStep = StepMode.Out;
            waitForContinue.Set();
        }

        public void Run()
        {
            state = DebuggerState.Running;
            nextStep = StepMode.Into;
            waitForContinue.Set();
        }

        public void Pause()
        {
            state = DebuggerState.Pausing;
        }

        public void ClearBreakpoints()
        {
            engine.DebugHandler.BreakPoints.Clear();
        }

        public Position SetBreakpoint(string sourceId, Position position, string condition)
        {
            var info = GetScriptInfo(sourceId);
            position = info.FindNearestBreakpointPosition(position);

            engine.DebugHandler.BreakPoints.Set(new BreakPoint(sourceId, position.Line, position.Column, condition));
            return position;
        }

        public void NotifyUIReady()
        {
            state = DebuggerState.Entering;
            waitForContinue.Set();
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
            scriptInfoBySourceId.Add(id, new ScriptInfo(ast));
        }

        private StepMode DebugHandler_Step(object sender, DebugInformation e)
        {
            cts.Token.ThrowIfCancellationRequested();

            if (!IsAttached)
            {
                return StepMode.Over;
            }

            switch (state)
            {
                case DebuggerState.WaitingForUI:
                    throw new InvalidOperationException("Debugger should not be stepping while waiting for UI");

                case DebuggerState.Entering:
                    if (!PauseOnEntry)
                    {
                        state = DebuggerState.Running;
                        return StepMode.Into;
                    }
                    state = DebuggerState.Stepping;
                    return OnPause(PauseReason.Entry, e);

                case DebuggerState.Running:
                    return StepMode.Into;

                case DebuggerState.Pausing:
                    state = DebuggerState.Stepping;
                    return OnPause(PauseReason.Pause, e);

                case DebuggerState.Stepping:
                    return OnPause(PauseReason.Step, e);

                case DebuggerState.Terminating:
                    throw new InvalidOperationException("Debugger should not be stepping while terminating");
                    return StepMode.Into;

                default:
                    throw new NotImplementedException($"Debugger state handling for {state} not implemented.");
            }

        }

        private StepMode DebugHandler_Break(object sender, DebugInformation e)
        {
            cts.Token.ThrowIfCancellationRequested();

            if (!IsAttached)
            {
                return StepMode.Over;
            }
            state = DebuggerState.Stepping;
            var reason = e.PauseType switch
            {
                PauseType.DebuggerStatement => PauseReason.DebuggerStatement,
                PauseType.Break or _ => PauseReason.Breakpoint
            };
            return OnPause(reason, e);
        }

        private StepMode OnPause(PauseReason reason, DebugInformation e)
        {
            CurrentDebugInformation = e;
            Stopped?.Invoke(reason, e);

            PauseThread();
            
            Continued?.Invoke();

            return nextStep;
        }

        private void PauseThread()
        {
            // Pause the thread until waitForContinue is set
            waitForContinue.WaitOne();
            waitForContinue.Reset();
        }
    }
}
