namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setVariable’ request.
    /// </summary>
    public class SetVariableResponse : ProtocolResponseBody
    {
        /// <param name="value">The new value of the variable.</param>
        public SetVariableResponse(string value)
        {
            Value = value;
        }

        /// <summary>
        /// The new value of the variable.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The type of the new value. Typically shown in the UI when hovering over the value.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// If variablesReference is > 0, the new value is structured and its children can be retrieved by passing
        /// variablesReference to the VariablesRequest. The value should be less than or equal to 2147483647 (2^31-1).
        /// </summary>
        public int? VariablesReference { get; set; }

        /// <summary>
        /// The number of named child variables.
        /// </summary>
        /// <remarks>
        /// The client can use this optional information to present the variables in a paged UI and fetch them
        /// in chunks. The value should be less than or equal to 2147483647 (2^31-1).
        /// </remarks>
        public int? NamedVariables { get; set; }

        /// <summary>
        /// The number of indexed child variables.
        /// </summary>
        /// <remarks>
        /// The client can use this optional information to present the variables in a paged UI and fetch them 
        /// in chunks. The value should be less than or equal to 2147483647 (2^31-1).
        /// </remarks>
        public int? IndexedVariables { get; set; }
    }
}
