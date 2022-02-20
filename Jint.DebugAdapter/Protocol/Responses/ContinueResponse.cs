namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ContinueResponse : ProtocolResponseBody
    {
        public bool? AllThreadsContinued { get; set; }
    }
}
