using System.Text.Json.Serialization;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// An ExceptionBreakpointsFilter is shown in the UI as an filter option for configuring how
    /// exceptions are dealt with.
    /// </summary>
    public class ExceptionBreakpointsFilter
    {
        /// <param name="filter">The internal ID of the filter option.</param>
        /// <param name="label">The name of the filter option.</param>
        [JsonConstructor]
        public ExceptionBreakpointsFilter(string filter, string label)
        {
            Filter = filter;
            Label = label;
        }

        /// <summary>
        /// The internal ID of the filter option. This value is passed to the 'setExceptionBreakpoints' request.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// The name of the filter option. This will be shown in the UI.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// An optional help text providing additional information about the exception filter.
        /// This string is typically shown as a hover and must be translated.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initial value of the filter option. If not specified a value 'false' is assumed.
        /// </summary>
        public bool? Default { get; set; }

        /// <summary>
        /// Controls whether a condition can be specified for this filter option. If false or missing, 
        /// a condition can not be set.
        /// </summary>
        public bool? SupportsCondition { get; set; }

        /// <summary>
        /// An optional help text providing information about the condition. This string is shown as the
        /// placeholder text for a text box and must be translated.
        /// </summary>
        public string ConditionDescription { get; set; }
    }
}
