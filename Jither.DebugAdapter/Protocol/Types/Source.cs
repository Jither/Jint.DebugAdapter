using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A Source is a descriptor for source code.
    /// </summary>
    /// <remarks>
    /// It is returned from the debug adapter as part of a StackFrame and it is used by clients when
    /// specifying breakpoints.
    /// </remarks>
    public class Source
    {
        /// <summary>
        /// The short name of the source. Every source returned from the debug adapter has a name.
        /// When sending a source to the debug adapter this name is optional.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The path of the source to be shown in the UI. It is only used to locate and load the content of the
        /// source if no sourceReference is specified (or its value is 0).
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// If sourceReference > 0 the contents of the source must be retrieved through the SourceRequest (even if
        /// a path is specified).
        /// </summary>
        /// <remarks>
        /// A sourceReference is only valid for a session, so it must not be used to persist 
        /// a source. The value should be less than or equal to 2147483647 (2^31-1).
        /// </remarks>
        public int? SourceReference { get; set; }

        /// <summary>
        /// An optional hint for how to present the source in the UI.
        /// </summary>
        /// <remarks>
        /// A value of 'deemphasize' can be used to indicate that the source is not available or that it is skipped 
        /// on stepping.
        /// </remarks>
        public SourcePresentationHint PresentationHint { get; set; }

        /// <summary>
        /// The (optional) origin of this source: possible values 'internal module', 'inlined content from source 
        /// map', etc.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// An optional list of sources that are related to this source. These may be the source that generated 
        /// this source.
        /// </summary>
        public List<Source> Sources { get; set; }

        /// <summary>
        /// Optional data that a debug adapter might want to loop through the client.
        /// </summary>
        /// <remarks>
        /// The client should leave the data intact and persist it across sessions.The client should not interpret
        /// the data.
        /// </remarks>
        public object AdapterData { get; set; }

        /// <summary>
        /// The checksums associated with this file.
        /// </summary>
        public List<Checksum> Checksums { get; set; }
    }
}
