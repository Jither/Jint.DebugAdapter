using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class VariablesArguments : ProtocolArguments
    {
        public int VariablesReference { get; set; }
        public string Filter { get; set; }
        public int? Start { get; set; }
        public int? Count { get; set; }
        public ValueFormat Format { get; set; }
    }
}
