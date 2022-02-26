using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that the target has produced some output.
    /// </summary>
    public class OutputEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "output";

        public OutputEvent(string output)
        {
            Output = output;
        }

        /// <summary>
        /// The output category. If not specified or if the category is not understood by the client,
        /// 'console' is assumed.
        /// </summary>
        public StringEnum<OutputCategory>? Category { get; set; }

        /// <summary>
        /// The output to report.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Support for keeping an output log organized by grouping related messages.
        /// </summary>
        public StringEnum<OutputGroup>? Group { get; set; }

        /// <summary>
        /// If an attribute 'variablesReference' exists and its value is > 0, the output contains objects which
        /// can be retrieved by passing 'variablesReference' to the 'variables' request.
        /// </summary>
        public int? VariablesReference { get; set; }

        /// <summary>
        /// An optional source location where the output was produced.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// An optional source location line where the output was produced.
        /// </summary>
        public int? Line { get; set; }

        /// <summary>
        /// An optional source location column where the output was produced.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// Optional data to report. For the 'telemetry' category the data will be sent to telemetry, 
        /// for the other categories the data is shown in JSON format.
        /// </summary>
        public object Data { get; set; }
    }
}
