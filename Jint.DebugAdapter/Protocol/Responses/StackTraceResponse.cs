using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    public class StackTraceResponse : ProtocolResponseBody
    {
        public List<StackFrame> StackFrames { get; set; }
        public int? TotalFrames { get; set; }
    }
}
