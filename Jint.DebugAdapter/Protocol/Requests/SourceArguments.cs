using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class SourceArguments : ProtocolArguments
    {
        public Source Source { get; set; }
        public int SourceReference { get; set; } // backwards compatibility
    }
}
