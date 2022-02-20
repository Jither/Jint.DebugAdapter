namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class RestartArguments : ProtocolArguments
    {
        // TODO: This is either AttachArguments or LaunchArguments
        public object Arguments { get; set; }
    }
}
