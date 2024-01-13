using System.Text.Json;
using Jint.DebugAdapter;

namespace Jint.DebugAdapterExample;

public class FilesScriptHost : IScriptHost
{
    public Engine Engine { get; }
    public ISourceProvider SourceProvider { get; }

    public FilesScriptHost()
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
        Engine.Modules.Import(program);
    }
}
