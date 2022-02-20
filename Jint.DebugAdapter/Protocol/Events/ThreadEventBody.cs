using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ThreadEventBody : ProtocolEventBody
    {
        public ThreadChangeReason Reason { get; set; }
        public int ThreadId { get; set; }
    }
}
