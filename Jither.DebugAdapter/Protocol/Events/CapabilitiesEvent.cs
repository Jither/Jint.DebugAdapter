using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that one or more capabilities have changed. This event has a hint characteristic.
    /// </summary>
    public class CapabilitiesEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "capabilities";

        public CapabilitiesEvent(Capabilities capabilities)
        {
            Capabilities = capabilities;
        }

        /// <summary>
        /// The set of updated capabilities.
        /// </summary>
        public Capabilities Capabilities { get; set; }
    }
}
