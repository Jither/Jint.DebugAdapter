using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public class FileSystemSourceProvider : ISourceProvider
    {
        public Source GetSource(string sourceId)
        {
            return new Source
            {
                Name = sourceId,
                Path = sourceId
            };
        }

        public string GetSourceId(Source source)
        {
            return source.Path;
        }
    }
}
