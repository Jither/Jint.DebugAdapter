namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Detailed information about an exception that has occurred.
    /// </summary>
    public class ExceptionDetails
    {
        /// <summary>
        /// Message contained in the exception.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Short type name of the exception object.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Fully-qualified type name of the exception object.
        /// </summary>
        public string FullTypeName { get; set; }

        /// <summary>
        /// Optional expression that can be evaluated in the current scope to obtain the exception object.
        /// </summary>
        public string EvaluateName { get; set; }

        /// <summary>
        /// Stack trace at the time the exception was thrown.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Details of the exception contained by this exception, if any.
        /// </summary>
        public List<ExceptionDetails> InnerException { get; set; }
    }
}
