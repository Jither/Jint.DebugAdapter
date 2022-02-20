using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ModuleEvent : ProtocolEventBody
    {
        public ChangeReason Reason { get; set; }
        public Module Module { get; set; }

        protected override string EventNameInternal => "memory";
    }
}
