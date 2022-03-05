using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘setExpression’ request.
    /// </summary>
    public class SetExpressionResponse : ProtocolResponseBody
    {
        /// <param name="value">The new value of the expression.</param>
        public SetExpressionResponse(string value)
        {
            Value = value;
        }

        /// <summary>
        /// The new value of the expression.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The optional type of the value.
        /// </summary>
        /// <remarks>
        /// This attribute should only be returned by a debug adapter if the client has passed the value true for
        /// the 'supportsVariableType' capability of the 'initialize' request.
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Properties of a value that can be used to determine how to render the result in the UI.
        /// </summary>
        public VariablePresentationHint PresentationHint { get; set; }

        /// <summary>
        /// If variablesReference is > 0, the value is structured and its children can be retrieved by passing
        /// variablesReference to the VariablesRequest. The value should be less than or equal to 2147483647 (2^31-1).
        /// </summary>
        public int? VariablesReference { get; set; }

        /// <summary>
        /// The number of named child variables.
        /// </summary>
        /// <remarks>
        /// The client can use this optional information to present the variables in a paged UI and fetch them in
        /// chunks. The value should be less than or equal to 2147483647 (2^31-1).
        /// </remarks>
        public int? NamedVariables { get; set; }

        /// <summary>
        /// The number of indexed child variables.
        /// </summary>
        /// <remarks>
        /// The client can use this optional information to present the variables in a paged UI and fetch them in 
        /// chunks. The value should be less than or equal to 2147483647 (2^31-1).
        /// </remarks>
        public int? IndexedVariables { get; set; }
    }
}
