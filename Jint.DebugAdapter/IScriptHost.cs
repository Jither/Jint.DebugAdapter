using System.Text.Json;

namespace Jint.DebugAdapter
{
    public interface IScriptHost
    {
        SourceProvider SourceProvider { get; }

        void Launch(string program, IReadOnlyDictionary<string, JsonElement> arguments);
    }
}
