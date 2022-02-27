using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="SourcePresentationHint"/>
    public class SourcePresentationHint : StringEnum<SourcePresentationHint>
    {
        public static readonly SourcePresentationHint Normal = Create("normal");
        public static readonly SourcePresentationHint Emphasize = Create("emphasize");
        public static readonly SourcePresentationHint Deemphasize = Create("deemphasize");
    }
}
