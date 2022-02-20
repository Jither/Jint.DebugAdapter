using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class LoadedSourcesResponseBody : ProtocolResponseBody
    {
        public List<Source> Sources { get; set; }
    }
}
