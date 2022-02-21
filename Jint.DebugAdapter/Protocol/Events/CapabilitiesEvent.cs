using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class CapabilitiesEvent : ProtocolEventBody
    {
        public Capabilities Capabilities { get; set; }

        protected override string EventNameInternal => "capabilities";
    }
}
