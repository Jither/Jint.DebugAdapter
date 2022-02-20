namespace Jint.DebugAdapter.Protocol.Types
{
    internal class ExceptionOptions
    {
        public List<ExceptionPathSegment> Path { get; set; }
        public ExceptionBreakMode BreakMode { get; set; }
    }
}
