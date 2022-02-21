using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    public class ProcessEvent : ProtocolEventBody
    {
        public string Name { get; set; }
        public int? SystemProcessId { get; set; }
        public bool? IsLocalProcess { get; set; }
        public StringEnum<StartMethod>? StartMethod { get; set; }
        public int? PointerSize { get; set; }

        protected override string EventNameInternal => "process";
    }
}
