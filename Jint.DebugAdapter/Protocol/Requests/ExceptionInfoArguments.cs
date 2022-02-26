namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Retrieves the details of the exception that caused this event to be raised.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsExceptionInfoRequest’ is true.
    /// </remarks>
    public class ExceptionInfoArguments : ProtocolArguments
    {
        /// <summary>
        /// Thread for which exception information should be retrieved.
        /// </summary>
        public int ThreadId { get; set; }
    }
}
