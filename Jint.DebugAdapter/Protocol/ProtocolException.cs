namespace Jint.DebugAdapter.Protocol
{
    internal class ProtocolException : Exception
    {
        public ProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
