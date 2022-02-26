namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Retrieves the set of all sources currently loaded by the debugged process.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsLoadedSourcesRequest’ is true.
    /// </remarks>
    public class LoadedSourcesArguments : ProtocolArguments
    {

    }
}
