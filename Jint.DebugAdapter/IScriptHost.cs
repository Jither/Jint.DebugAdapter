using System.Text.Json;

namespace Jint.DebugAdapter
{
    public interface IScriptHost
    {
        ISourceProvider SourceProvider { get; }

        void Launch(string program, IReadOnlyDictionary<string, JsonElement> arguments);
    }
}
