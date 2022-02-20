namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ContinueResponseBody : ProtocolResponseBody
    {
        public bool? AllThreadsContinued { get; set; }
    }
}
