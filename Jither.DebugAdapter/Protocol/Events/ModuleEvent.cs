using Jither.DebugAdapter.Helpers;
using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that some information about a module has changed.
    /// </summary>
    public class ModuleEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "memory";

        public ModuleEvent(ChangeReason reason, Module module)
        {
            Reason = reason;
            Module = module;
        }

        /// <summary>
        /// The reason for the event.
        /// </summary>
        public ChangeReason Reason { get; set; }

        /// <summary>
        /// The new, changed, or removed module. In case of 'removed' only the module id is used.
        /// </summary>
        public Module Module { get; set; }
    }
}
