namespace Jint.DebugAdapter.Protocol.Types
{
    public class ExceptionPathSegment
    {
        public bool? Negate { get; set; }
        public List<string> Names { get; set; }
    }
}
