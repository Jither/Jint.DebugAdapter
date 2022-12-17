using System.Text.Json;
using Jint.DebugAdapter;

namespace Jint.DebugAdapterExample
{
    public class InternalScriptHost : IScriptHost
    {
        public Engine Engine { get; }
        public ISourceProvider SourceProvider { get; }

        private Dictionary<string, string> scriptsBySourceId = new()
        {
            ["main"] = @"function main()
{
    console.log('Hello from an internal script!');
}

main()"
        };

        public InternalScriptHost()
        {
            var provider = new InternalSourceProvider();

            provider.Register("Main script", "main", scriptsBySourceId["main"]);

            SourceProvider = provider;

            Engine = new Engine(options =>
            {
                options.DebugMode(true)
                    .SetupDebugger();
            });
        }

        public void RegisterConsole(DebugAdapter.Console console)
        {
            Engine.SetValue("console", console);
        }

        public void Launch(string program, IReadOnlyDictionary<string, JsonElement> arguments)
        {
            var script = scriptsBySourceId.GetValueOrDefault(program);
            if (script == null)
            {
                throw new Exception($"Unknown script: {program}");
            }
            Engine.Execute(script, program);
        }
    }
}
