using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ModuleEventBody : ProtocolEventBody
    {
        public ChangeReason Reason { get; set; }
        public Module Module { get; set; }
    }
}
