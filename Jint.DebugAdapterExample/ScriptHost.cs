using System.Text.Json;
using Jint.DebugAdapter;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapterExample
{
    public class ScriptHost : IScriptHost
    {
        public Engine Engine { get; }
        public SourceProvider SourceProvider { get; }

        public ScriptHost()
        {
            SourceProvider = new SourceProvider();

            Engine = new Engine(options =>
            {
                options.DebugMode(true)
                    .SetupDebugger()
                    .EnableModules(@"D:\Web\test");
            });
        }

        public void RegisterConsole(DebugAdapter.Console console)
        {
            Engine.SetValue("console", console);
        }

        public void Launch(string program, IReadOnlyDictionary<string, JsonElement> arguments)
        {
            string source = File.ReadAllText(program);
            string sourceId = SourceProvider.Register(program);
            Engine.Execute(source, new Esprima.ParserOptions(sourceId));
        }
    }
}
