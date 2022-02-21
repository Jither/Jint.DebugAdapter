using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ScopesResponse : ProtocolResponseBody
    {
        public List<Scope> Scopes { get; set; }
    }
}
