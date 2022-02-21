using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class ExceptionOptions
    {
        public List<ExceptionPathSegment> Path { get; set; }
        public StringEnum<ExceptionBreakMode> BreakMode { get; set; }
    }
}
