using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esprima.Ast;
using Jint.Native;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    public delegate void DebugPauseEventHandler(PauseReason reason, DebugInformation info);
    public delegate void DebugContinueEventHandler();

    public enum PauseReason
    {
        Step,
        Breakpoint,
        Exception,
        Pause,
        Entry
    }

    internal enum DebuggerState
    {
        Entering,
        Running,
        Pausing,
        Stepping,
    }

    public class Debugger
    {
        private readonly Dictionary<string, Script> astBySourceId = new();
        private readonly Engine engine;
        private readonly ManualResetEvent waitForContinue = new(false);
        private StepMode nextStep;
        private DebuggerState state;
        private bool pauseOnEntry;

        public bool IsAttached { get; private set; }
        public bool IsStopped { get; private set; }

        public event DebugPauseEventHandler Stop;
        public event DebugContinueEventHandler Continue;

        public Debugger(Engine engine)
        {
            this.engine = engine;
            engine.Parsed += Engine_Parsed;
            engine.DebugHandler.Break += DebugHandler_Break;
            engine.DebugHandler.Step += DebugHandler_Step;
        }

        public async Task<JsValue> ExecuteAsync(string path, bool noDebug = false, bool pauseOnEntry = false)
        {
            this.pauseOnEntry = pauseOnEntry;

            var script = File.ReadAllText(path);
            IsAttached = true;
            IsStopped = false;
            state = DebuggerState.Entering;
            try
            {
                var result = await Task.Run(() =>
                {
                    return engine.Evaluate(script, parserOptions: new Esprima.ParserOptions(path) { Tokens = true });
                });
                return result;
            }
            finally
            {
                IsAttached = false;
                IsStopped = false;
            }
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
            nextStep = StepMode.None;
            waitForContinue.Set();
        }

        public void Pause()
        {
            state = DebuggerState.Pausing;
        }

        private void Engine_Parsed(object sender, SourceParsedEventArgs e)
        {
            // Whenever the engine parses a script (but before it's executed), this event handler is called,
            // allowing us to store the script's AST and source ID. We use this for e.g. verifying breakpoint
            // locations.
            astBySourceId.Add(e.SourceId, e.Ast);
        }

        private StepMode DebugHandler_Step(object sender, DebugInformation e)
        {
            if (!IsAttached)
            {
                return StepMode.Over;
            }

            switch (state)
            {
                case DebuggerState.Entering:
                    if (!pauseOnEntry)
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
            if (!IsAttached)
            {
                return StepMode.Over;
            }
            return OnPause(PauseReason.Breakpoint, e);
        }

        private StepMode OnPause(PauseReason reason, DebugInformation e)
        {
            IsStopped = true;
            Stop?.Invoke(reason, e);
            
            // Pause the thread until waitForContinue is set
            waitForContinue.WaitOne();
            waitForContinue.Reset();
            
            IsStopped = false;
            Continue?.Invoke();

            return nextStep;
        }
    }
}
