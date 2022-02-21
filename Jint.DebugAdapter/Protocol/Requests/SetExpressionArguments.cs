using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class SetExpressionArguments : ProtocolArguments
    {
        public string Expression { get; set; }
        public string Value { get; set; }
        public int? FrameId { get; set; }
        public ValueFormat Format { get; set; }
    }
}
