using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    public class ModuleEvent : ProtocolEventBody
    {
        public StringEnum<ChangeReason> Reason { get; set; }
        public Module Module { get; set; }

        protected override string EventNameInternal => "memory";
    }
}
