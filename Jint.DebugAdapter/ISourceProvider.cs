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
    }
}
