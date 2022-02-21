using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class LoadedSourceEvent : ProtocolEventBody
    {
        public StringEnum<ChangeReason> Reason { get; set; }
        public Source Source { get; set; }

        protected override string EventNameInternal => "loadedSource";
    }
}
