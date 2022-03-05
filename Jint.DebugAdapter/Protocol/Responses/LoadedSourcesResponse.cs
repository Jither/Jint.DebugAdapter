using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘loadedSources’ request.
    /// </summary>
    public class LoadedSourcesResponse : ProtocolResponseBody
    {
        /// <param name="sources">Set of loaded sources.</param>
        public LoadedSourcesResponse(IEnumerable<Source> sources)
        {
            Sources = sources;
        }

        /// <summary>
        /// Set of loaded sources.
        /// </summary>
        public IEnumerable<Source> Sources { get; set; }
    }
}
