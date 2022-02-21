using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class EvaluateArguments : ProtocolArguments
    {
        public string Expression { get; set; }
        public int? FrameId { get; set; }
        public EvaluationContext? Context { get; set; }
        public ValueFormat Format { get; set; }
    }
}
