using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class OutputEvent : ProtocolEventBody
    {
        public StringEnum<OutputCategory>? Category { get; set; }
        public string Output { get; set; }
        public StringEnum<OutputGroup>? Group { get; set; }
        public int? VariablesReference { get; set; }
        public Source Source { get; set; }
        public int? Line { get; set; }
        public int? Column { get; set; }
        public object Data { get; set; }

        protected override string EventNameInternal => "output";
    }
}
