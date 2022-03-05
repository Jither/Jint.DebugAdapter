using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘dataBreakpointInfo’ request.
    /// </summary>
    public class DataBreakpointInfoResponse : ProtocolResponseBody
    {
        /// <param name="dataId">An identifier for the data on which a data breakpoint can be registered with
        /// the setDataBreakpoints request or null if no data breakpoint is available.</param>
        /// <param name="description">UI string that describes on what data the breakpoint is set on or why a
        /// data breakpoint is not available.</param>
        public DataBreakpointInfoResponse(string dataId, string description)
        {
            DataId = dataId;
            Description = description;
        }

        /// <summary>
        /// An identifier for the data on which a data breakpoint can be registered with
        /// the setDataBreakpoints request or null if no data breakpoint is available.
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// UI string that describes on what data the breakpoint is set on or why a
        /// data breakpoint is not available.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Optional attribute listing the available access types for a potential data breakpoint.
        /// </summary>
        /// <remarks>
        /// A UI frontend could surface this information.
        /// </remarks>
        public IEnumerable<DataBreakpointAccessType> AccessTypes { get; set; }

        /// <summary>
        /// Optional attribute indicating that a potential data breakpoint
        /// could be persisted across sessions.
        /// </summary>
        public bool? CanPersist { get; set; }
    }
}
