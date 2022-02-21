using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class StackTraceArguments : ProtocolArguments
    {
        public int ThreadId { get; set; }
        public int? StartFrame { get; set; }
        public int? Levels { get; set; }
        public StackFrameFormat Format { get; set; }
    }
}
