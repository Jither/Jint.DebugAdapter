using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public interface ISourceProvider
    {
        /// <summary>
        /// Returns the source ID for a given DAP Source
        /// </summary>
        string GetSourceId(Source source);

        /// <summary>
        /// Returns DAP Source for a given source ID
        /// </summary>
        Source GetSource(string sourceId);

        /// <summary>
        /// Returns script content for a given DAP source. This may throw if provider doesn't use
        /// DAP script references (integer IDs) - e.g. a file system-based provider returning paths
        /// </summary>
        string GetContent(Source source);
    }
}
