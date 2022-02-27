using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Properties of a data breakpoint passed to the setDataBreakpoints request.
    /// </summary>
    public class DataBreakpoint
    {
        /// <param name="dataId">An id representing the data. This id is returned from the
        /// dataBreakpointInfo request.</param>
        [JsonConstructor]
        public DataBreakpoint(string dataId)
        {
            DataId = dataId;
        }

        /// <summary>
        /// An id representing the data. This id is returned from the dataBreakpointInfo request.
        /// </summary>
        public string DataId { get; set; }

        // This isn't a StringEnum - it has the members it's likely to ever have, and is also used in a List in DataBreakpointInfoResponse
        /// <summary>
        /// The access type of the data.
        /// </summary>
        public DataBreakpointAccessType? AccessType { get; set; }

        /// <summary>
        /// An optional expression for conditional breakpoints.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// An optional expression that controls how many hits of the breakpoint are ignored.
        /// The backend is expected to interpret the expression as needed.
        /// </summary>
        public string HitCondition { get; set; }
    }
}
