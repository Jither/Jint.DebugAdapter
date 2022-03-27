using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public class FileSystemSourceProvider : ISourceProvider
    {
        public string GetContent(Source source)
        {
            // We don't provide the content - the file system does.
            throw new NotImplementedException();
        }

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
