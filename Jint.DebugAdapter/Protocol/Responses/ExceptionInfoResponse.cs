using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ExceptionInfoResponse : ProtocolResponseBody
    {
        public string ExceptionId { get; set; }
        public string Description { get; set; }
        public ExceptionBreakMode BreakMode { get; set; }
        public ExceptionDetails ExceptionDetails { get; set; }
    }
}
