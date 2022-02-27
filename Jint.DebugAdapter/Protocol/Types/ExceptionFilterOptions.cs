using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// An ExceptionFilterOptions is used to specify an exception filter together with a condition for the
    /// setExceptionsFilter request.
    /// </summary>
    public class ExceptionFilterOptions
    {
        /// <param name="filterId">ID of an exception filter returned by the 'exceptionBreakpointFilters'
        /// capability.</param>
        [JsonConstructor]
        public ExceptionFilterOptions(string filterId)
        {
            FilterId = filterId;
        }

        /// <summary>
        /// ID of an exception filter returned by the 'exceptionBreakpointFilters' capability.
        /// </summary>
        public string FilterId { get; set; }

        /// <summary>
        /// An optional expression for conditional exceptions. The exception will break into the debugger if the 
        /// result of the condition is true.
        /// </summary>
        public string Condition { get; set; }
    }
}
