namespace Jint.DebugAdapter.Protocol.Types
{
    public class ExceptionDetails
    {
        public string Message { get; set; }
        public string TypeName { get; set; }
        public string FullTypeName { get; set; }
        public string EvaluateName { get; set; }
        public string StackTrace { get; set; }
        public List<ExceptionDetails> InnerException { get; set; }
    }
}
