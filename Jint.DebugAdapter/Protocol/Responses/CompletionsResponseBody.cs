using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class CompletionsResponseBody : ProtocolResponseBody
    {
        public List<CompletionItem> Targets { get; set; }
    }
}
