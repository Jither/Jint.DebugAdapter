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
        private const string EvaluationId = "<<<evaluation>>>";
        private readonly Dictionary<string, ScriptInfo> scriptInfoBySourceId = new();
        private readonly Engine engine;
        private readonly ManualResetEvent waitForContinue = new(false);
        private readonly CancellationTokenSource cts = new();
        private StepMode nextStep;
        private DebuggerState state;
        private bool pauseOnEntry;

        public bool IsAttached { get; private set; }
        public bool IsStopped { get; private set; }
        public DebugInformation CurrentDebugInformation { get; private set; }
        public Engine Engine => engine;

        public event DebugPauseEventHandler Stopped;
        public event DebugEventHandler Continued;
        public event DebugEventHandler Cancelled;
        public event DebugEventHandler Finished;

        public Debugger(Engine engine)
        {
            this.engine = engine;
            engine.Parsed += Engine_Parsed;
            engine.DebugHandler.Break += DebugHandler_Break;
            engine.DebugHandler.Step += DebugHandler_Step;
        }

        public ScriptInfo GetScriptInfo(string id)
        {
            return scriptInfoBySourceId.GetValueOrDefault(id);
        }

        public void Prepare(string id, string path)
        {
            var script = File.ReadAllText(path);
            var parser = new JavaScriptParser(script, new ParserOptions(id) { Tokens = true });
            var ast = parser.ParseScript();
            RegisterScriptInfo(id, ast);
        }

        public async Task ExecuteAsync(string id, bool noDebug = false, bool pauseOnEntry = false)
        {
            var ast = GetScriptInfo(id).Ast;

            this.pauseOnEntry = pauseOnEntry;

            IsAttached = !noDebug;
            IsStopped = false;
            state = DebuggerState.Preparing;
            try
            {
                var result = await Task.Run(() =>
                {
                    return engine.Evaluate(ast);
                }, cts.Token);
                Finished?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Cancelled?.Invoke();
            }
            finally
            {
                IsAttached = false;
                IsStopped = false;
            }
        }

        public JsValue Evaluate(string expression)
        {
            return engine.Evaluate(expression, new ParserOptions(EvaluationId));
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

        private void Engine_Parsed(object sender, SourceParsedEventArgs e)
        {
            // Whenever the engine parses a script (but before it's executed), this event handler is called,
            // allowing us to store the script's AST and source ID. We use this for e.g. verifying breakpoint
            // locations.
            // However, if this is the debugger evaluating a script, we don't need (or want) to register it:
            if (e.SourceId == EvaluationId)
            {
                return;
            }
            RegisterScriptInfo(e.SourceId, e.Ast);
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
                case DebuggerState.Preparing:

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
            cts.Token.ThrowIfCancellationRequested();

            if (!IsAttached)
            {
                return StepMode.Over;
            }
            state = DebuggerState.Stepping;
            return OnPause(PauseReason.Breakpoint, e);
        }

        private StepMode OnPause(PauseReason reason, DebugInformation e)
        {
            IsStopped = true;
            CurrentDebugInformation = e;
            Stopped?.Invoke(reason, e);
            
            // Pause the thread until waitForContinue is set
            waitForContinue.WaitOne();
            waitForContinue.Reset();
            
            IsStopped = false;
            Continued?.Invoke();

            return nextStep;
        }
    }
}
