namespace Jint.DebugAdapter.Protocol.Types
{
    internal class ExceptionPathSegment
    {
        public bool? Negate { get; set; }
        public List<string> Names { get; set; }
    }
}
