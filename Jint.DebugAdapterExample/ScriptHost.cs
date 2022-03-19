using System.Text.Json;
using Jint.DebugAdapter;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapterExample
{
    public class ScriptHost : IScriptHost
    {
        private readonly Engine engine;
        public SourceProvider SourceProvider { get; }

        public Debugger Debugger { get; }

        public ScriptHost()
        {
            SourceProvider = new SourceProvider();

            engine = new Engine(options =>
            {
                options.DebugMode(true)
                    // In order to stop on entry, we need to be stepping from the start
                    // The Debugger will change to StepMode.None if not stopping on entry.
                    .InitialStepMode(StepMode.Into)
                    .DebuggerStatementHandling(DebuggerStatementHandling.Script)
                    .EnableModules(@"D:\Web\test");
            });

            Debugger = new Debugger(engine);
        }

        public void RegisterConsole(DebugAdapter.Console console)
        {
            engine.SetValue("console", console);
        }

        public void Launch(string program, bool debug, IReadOnlyDictionary<string, JsonElement> arguments)
        {
            string source = File.ReadAllText(program);
            string sourceId = SourceProvider.Register(program);
            Debugger.Execute(sourceId, source, debug);
        }
    }
}
