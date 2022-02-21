using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class LoadedSourcesResponse : ProtocolResponseBody
    {
        public List<Source> Sources { get; set; }
    }
}
