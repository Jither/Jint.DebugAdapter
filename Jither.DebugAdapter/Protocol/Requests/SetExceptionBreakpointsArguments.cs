using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// The request configures the debuggers response to thrown exceptions.
    /// </summary>
    /// <remarks>
    /// If an exception is configured to break, a ‘stopped’ event is fired (with reason ‘exception’).
    /// Clients should only call this request if the capability ‘exceptionBreakpointFilters’ returns
    /// one or more filters.
    /// </remarks>
    public class SetExceptionBreakpointsArguments : ProtocolArguments
    {
        /// <summary>
        /// Set of exception filters specified by their ID.
        /// </summary>
        /// <remarks>
        /// The set of all possible exception filters is defined by the 'exceptionBreakpointFilters'
        /// capability.The 'filter' and 'filterOptions' sets are additive.
        /// </remarks>
        public List<string> Filters { get; set; }

        /// <summary>
        /// Set of exception filters and their options.
        /// </summary>
        /// <remarks>
        /// The set of all possible exception filters is defined by the 'exceptionBreakpointFilters'
        /// capability.This attribute is only honored by a debug adapter if the capability 
        /// 'supportsExceptionFilterOptions' is true. The 'filter' and 'filterOptions' sets are additive.
        /// </remarks>
        public List<ExceptionFilterOptions> FilterOptions { get; set; }

        /// <summary>
        /// Configuration options for selected exceptions.
        /// </summary>
        /// <remarks>
        /// The attribute is only honored by a debug adapter if the capability 'supportsExceptionOptions' is true.
        /// </remarks>
        public List<ExceptionOptions> ExceptionOptions { get; set; }
    }
}
