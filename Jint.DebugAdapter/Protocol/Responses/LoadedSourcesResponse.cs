using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class LoadedSourcesResponse : ProtocolResponseBody
    {
        public List<Source> Sources { get; set; }
    }
}
