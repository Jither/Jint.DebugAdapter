using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class EvaluateArguments : ProtocolArguments
    {
        public string Expression { get; set; }
        public int? FrameId { get; set; }
        public StringEnum<EvaluationContext>? Context { get; set; }
        public ValueFormat Format { get; set; }
    }
}
