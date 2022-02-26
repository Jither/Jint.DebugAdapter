namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// This optional request indicates that the client has finished initialization of the debug adapter.
    /// </summary>
    /// <remarks>
    /// It is the last request in the sequence of configuration requests (which was started by the ‘initialized’ event).
    /// Clients should only call this request if the capability ‘supportsConfigurationDoneRequest’ is true.
    /// </remarks>
    public class ConfigurationDoneArguments : ProtocolArguments
    {

    }
}
