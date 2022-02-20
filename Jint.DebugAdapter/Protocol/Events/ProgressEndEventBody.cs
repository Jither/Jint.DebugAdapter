namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ProgressEndEventBody : ProtocolEventBody
    {
        public string ProgressId { get; set; }
        public string Message { get; set; }
    }
}
