using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class LoadedSourceEventBody : ProtocolEventBody
    {
        public ChangeReason Reason { get; set; }
        public Source Source { get; set; }
    }
}
