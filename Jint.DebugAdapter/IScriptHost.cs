using System.Text.Json;

namespace Jint.DebugAdapter
{
    public interface IScriptHost
    {
        Debugger Debugger { get; }
        void Launch(string program, bool debug, IReadOnlyDictionary<string, JsonElement> arguments);
        SourceProvider SourceProvider { get; }
    }
}
