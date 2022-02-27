using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setExceptionBreakpoints’ request.
    /// </summary>
    /// <remarks>
    /// The mandatory ‘verified’ property of a Breakpoint object signals whether the exception
    /// breakpoint or filter could be successfully created and whether the optional condition or
    /// hit count expressions are valid. In case of an error the ‘message’ property explains the
    /// problem. An optional ‘id’ property can be used to introduce a unique ID for the exception
    /// breakpoint or filter so that it can be updated subsequently by sending breakpoint events.
    ///     
    /// For backward compatibility both the ‘breakpoints’ array and the enclosing ‘body’
    /// are optional. If these elements are missing a client will not be able to show problems
    /// for individual exception breakpoints or filters.
    /// </remarks>
    public class SetExceptionBreakpointsResponse : ProtocolResponseBody
    {
        /// <param name="breakpoints">Information about the exception breakpoints or filters. The breakpoints
        /// returned are in the same order as the elements of the 'filters', 'filterOptions', 'exceptionOptions'
        /// arrays in the arguments. If both 'filters' and 'filterOptions' are given, the returned array must
        /// start with 'filters' information first, followed by 'filterOptions' information.</param>
        public SetExceptionBreakpointsResponse(List<Breakpoint> breakpoints)
        {
            Breakpoints = breakpoints;
        }

        /// <summary>
        /// Information about the exception breakpoints or filters.
        /// </summary>
        /// <remarks>
        /// The breakpoints returned are in the same order as the elements of the
        /// 'filters', 'filterOptions', 'exceptionOptions' arrays in the arguments.
        /// If both 'filters' and 'filterOptions' are given, the returned array must
        /// start with 'filters' information first, followed by 'filterOptions'
        /// information.
        /// </remarks>
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
