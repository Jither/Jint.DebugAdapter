using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that some source has been added, changed, or removed from the set of all loaded sources.
    /// </summary>
    public class LoadedSourceEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "loadedSource";

        public LoadedSourceEvent(StringEnum<ChangeReason> reason, Source source)
        {
            Reason = reason;
            Source = source;
        }

        /// <summary>
        /// The reason for the event.
        /// </summary>
        public StringEnum<ChangeReason> Reason { get; set; }

        /// <summary>
        /// The new, changed, or removed source.
        /// </summary>
        public Source Source { get; set; }
    }
}
