using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class OutputEventBody : ProtocolEventBody
    {
        public OutputCategory? Category { get; set; }
        public string Output { get; set; }
        public OutputGroup? Group { get; set; }
        public int? VariablesReference { get; set; }
        public Source Source { get; set; }
        public int? Line { get; set; }
        public int? Column { get; set; }
        public object Data { get; set; }
    }
}
