using System.Text.Json;
using Jint.DebugAdapter;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapterExample
{
    public class RunningScriptHost : IScriptHost
    {
        public Engine Engine { get; }
        public ISourceProvider SourceProvider { get; }

        public RunningScriptHost()
        {
            SourceProvider = new FileSystemSourceProvider();

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
            // In this case, Launch is called immediately on program start
            var fullPath = Path.GetFullPath(program);
            var script = File.ReadAllText(fullPath);
            Engine.Execute(script, fullPath);
        }
    }
}
