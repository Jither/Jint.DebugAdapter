namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class RunInTerminalResponse : ProtocolResponseBody
    {
        public int? ProcessId { get; set; }
        public int? ShellProcessId { get; set; }
    }
}
