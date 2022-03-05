using Jither.DebugAdapter.Helpers;
using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that some source has been added, changed, or removed from the set of all loaded sources.
    /// </summary>
    public class LoadedSourceEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "loadedSource";

        public LoadedSourceEvent(ChangeReason reason, Source source)
        {
            Reason = reason;
            Source = source;
        }

        /// <summary>
        /// The reason for the event.
        /// </summary>
        public ChangeReason Reason { get; set; }

        /// <summary>
        /// The new, changed, or removed source.
        /// </summary>
        public Source Source { get; set; }
    }
}
