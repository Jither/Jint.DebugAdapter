using Jither.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter
{
    public class InternalSourceProvider : ISourceProvider
    {
        private class SourceReference
        {
            public string SourceId { get; }
            public string Name { get; }
            public int Reference { get; }
            public string Script { get; }

            public SourceReference(string name, string sourceId, int reference, string script)
            {
                Name = name;
                SourceId = sourceId;
                Reference = reference;
                Script = script;
            }
        }

        private Dictionary<string, SourceReference> referencesBySourceId = new();
        private Dictionary<int, SourceReference> referencesByRefId = new();

        public int Register(string name, string sourceId, string script)
        {
            int referenceId = referencesByRefId.Count + 1;
            var reference = new SourceReference(name, sourceId, referenceId, script);
            referencesBySourceId.Add(sourceId, reference);
            referencesByRefId.Add(referenceId, reference);
            return referenceId;
        }

        public Source GetSource(string sourceId)
        {
            if (referencesBySourceId.TryGetValue(sourceId, out var result))
            {
                return new Source
                {
                    Name = result.Name,
                    SourceReference = result.Reference
                };
            }
            // For now, if we don't find a reference, we assume it's a file system source
            return new Source
            {
                Name = sourceId,
                Path = sourceId
            };
        }

        public string GetSourceId(Source source)
        {
            if (source.SourceReference < 1)
            {
                return null;
            }
            
            return GetReference(source).SourceId;
        }

        public string GetContent(Source source)
        {
            if (source.SourceReference < 1)
            {
                throw new ArgumentException($"Attempt to get script content, but reference wasn't provided.");
            }
            return GetReference(source).Script;
        }

        private SourceReference GetReference(Source source)
        {
            if (!referencesByRefId.TryGetValue(source.SourceReference.Value, out var reference))
            {
                throw new ArgumentException($"Unknown source for source reference {source.SourceReference}");
            }
            return reference;
        }
    }
}
