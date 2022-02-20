using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class DataBreakpointInfoResponseBody : ProtocolResponseBody
    {
        public string DataId { get; set; }
        public string Description { get; set; }
        public List<DataBreakpointAccessType> AccessTypes { get; set; }
        public bool? CanPersist { get; set; }
    }
}
