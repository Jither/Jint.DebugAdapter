using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="ChecksumAlgorithm"/>
    public class ChecksumAlgorithm : StringEnum<ChecksumAlgorithm>
    {
        public static readonly ChecksumAlgorithm MD5 = Create("MD5");
        public static readonly ChecksumAlgorithm SHA1 = Create("SHA1");
        public static readonly ChecksumAlgorithm SHA256 = Create("SHA256");
        public static readonly ChecksumAlgorithm Timestamp = Create("timestamp");
    }
}
