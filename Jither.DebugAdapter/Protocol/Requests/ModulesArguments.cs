namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Modules can be retrieved from the debug adapter with this request which can either return all modules or a
    /// range of modules to support paging.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsModulesRequest’ is true.
    /// </remarks>
    public class ModulesArguments : ProtocolArguments
    {
        /// <summary>
        /// The index of the first module to return; if omitted modules start at 0.
        /// </summary>
        public int? StartModule { get; set; }

        /// <summary>
        /// The number of modules to return. If moduleCount is not specified or 0, all modules are returned.
        /// </summary>
        public int? ModuleCount { get; set; }
    }
}
