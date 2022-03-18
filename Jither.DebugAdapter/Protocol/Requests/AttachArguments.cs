namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The attach request is sent from the client to the debug adapter to attach to a debuggee that is
    /// already running.
    /// </summary>
    /// <remarks>
    /// Since attaching is debugger/runtime specific, the arguments for this request are not part
    /// of this specification.
    /// </remarks>
    public class AttachArguments : ConfigurationArguments
    {
        
    }
}
