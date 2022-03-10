using System.Text.Json;

namespace Jint.DebugAdapter
{
    public interface IScriptHost
    {
        void Launch(string program, bool debug, IReadOnlyDictionary<string, JsonElement> arguments);
        SourceProvider SourceProvider { get; }
    }
}
