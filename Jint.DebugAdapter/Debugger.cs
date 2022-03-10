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
        }

        private readonly Dictionary<string, ScriptInfo> scriptInfoBySourceId = new();
        private readonly Engine engine;
        private readonly ManualResetEvent waitForContinue = new(false);
        private readonly CancellationTokenSource cts = new();
        private StepMode nextStep;
        private DebuggerState state = DebuggerState.WaitingForUI;

        public bool PauseOnEntry { get; set; }
        public bool IsAttached { get; private set; }
        public DebugInformation CurrentDebugInformation { get; private set; }
        public Engine Engine => engine;

        public event DebugEventHandler Ready;
        public event DebugPauseEventHandler Stopped;
        public event DebugEventHandler Continued;

        public Debugger(Engine engine)
        {
            this.engine = engine;
        }

        public void Attach()
        {
            if (IsAttached)
            {
                throw new InvalidOperationException($"Attempt to attach debugger when already attached.");
            }
            IsAttached = true;
            engine.Parsed += Engine_Parsed;
            engine.DebugHandler.Break += DebugHandler_Break;
            engine.DebugHandler.Step += DebugHandler_Step;
        }

        public void Detach()
        {
            if (!IsAttached)
            {
                return;
            }
            engine.Parsed -= Engine_Parsed;
            engine.DebugHandler.Break -= DebugHandler_Break;
            engine.DebugHandler.Step -= DebugHandler_Step;
            IsAttached = false;
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

        private void Engine_Parsed(object sender, SourceParsedEventArgs e)
        {
            // Whenever the engine parses a script (but before it's executed), this event handler is called,
            // allowing us to store the script's AST and source ID. We use this for e.g. verifying breakpoint
            // locations.
            RegisterScriptInfo(e.SourceId, e.Ast);

            // And we pause the engine thread, to wait for the debugger UI
            if (state == DebuggerState.WaitingForUI)
            {
                Ready?.Invoke();
                PauseThread();
            }
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
                    throw new InvalidOperationException($"Debugger should not be stepping while waiting for UI");

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
