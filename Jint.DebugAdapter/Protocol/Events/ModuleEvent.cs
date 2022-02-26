using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that some information about a module has changed.
    /// </summary>
    public class ModuleEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "memory";

        public ModuleEvent(StringEnum<ChangeReason> reason, Module module)
        {
            Reason = reason;
            Module = module;
        }

        /// <summary>
        /// The reason for the event.
        /// </summary>
        public StringEnum<ChangeReason> Reason { get; set; }

        /// <summary>
        /// The new, changed, or removed module. In case of 'removed' only the module id is used.
        /// </summary>
        public Module Module { get; set; }
    }
}
