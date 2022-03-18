using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter
{
    // TODO: This should eventually be replaced with a more general purpose SourceProvider for both filesystem
    // scripts and other script sources.
    public class SourceProvider
    {
        private readonly Dictionary<string, string> pathsBySourceId = new();
        private readonly Dictionary<string, string> sourceIdsByPath = new();

        public string Register(string path)
        {
            if (!sourceIdsByPath.TryGetValue(path, out string sourceId))
            {
                sourceId = Guid.NewGuid().ToString();
                pathsBySourceId.Add(sourceId, path);
                sourceIdsByPath.Add(path, sourceId);
            }

            return sourceId;
        }

        public string GetSourceId(string path)
        {
            if (!sourceIdsByPath.TryGetValue(path, out string id))
            {
                throw new DebuggerException($"Source ID for path '{path}' not found.");
            }
            return id;
        }

        public string GetSourcePath(string sourceId)
        {
            if (!pathsBySourceId.TryGetValue(sourceId, out string path))
            {
                throw new DebuggerException($"Path for Source ID '{sourceId}' not found.");
            }
            return path;
        }
    }
}
