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
            var sourceId = Guid.NewGuid().ToString();
            pathsBySourceId.Add(sourceId, path);
            sourceIdsByPath.Add(path, sourceId);

            return sourceId;
        }

        public string GetSourceId(string path)
        {
            return sourceIdsByPath.GetValueOrDefault(path);
        }

        public string GetSourcePath(string sourceId)
        {
            return pathsBySourceId.GetValueOrDefault(sourceId);
        }
    }
}
