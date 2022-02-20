namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class RunInTerminalResponseBody : ProtocolResponseBody
    {
        public int? ProcessId { get; set; }
        public int? ShellProcessId { get; set; }
    }
}
