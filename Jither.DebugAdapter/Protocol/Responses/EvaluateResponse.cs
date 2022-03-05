using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘evaluate’ request.
    /// </summary>
    public class EvaluateResponse : ProtocolResponseBody
    {
        /// <param name="result">The result of the evaluate request.</param>
        public EvaluateResponse(string result)
        {
            Result = result;
        }

        /// <summary>
        /// The result of the evaluate request.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// The optional type of the evaluate result.
        /// </summary>
        /// <remarks>
        /// This attribute should only be returned by a debug adapter if the client has passed the value true for
        /// the 'supportsVariableType' capability of the 'initialize' request.
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Properties of a evaluate result that can be used to determine how to render the result in the UI.
        /// </summary>
        public VariablePresentationHint PresentationHint { get; set; }

        /// <summary>
        /// If variablesReference is > 0, the evaluate result is structured and its children can be retrieved by
        /// passing variablesReference to the VariablesRequest. The value should be less than or equal to 
        /// 2147483647 (2^31-1).
        /// </summary>
        public int VariablesReference { get; set; }

        /// <summary>
        /// The number of named child variables.The client can use this optional information to present the
        /// variables in a paged UI and fetch them in chunks. The value should be less than or equal to
        /// 2147483647 (2^31-1).
        /// </summary>
        public int? NamedVariables { get; set; }

        /// <summary>
        /// The number of indexed child variables. The client can use this optional information to present the
        /// variables in a paged UI and fetch them in chunks. The value should be less than or equal to
        /// 2147483647 (2^31-1).
        /// </summary>
        public int? IndexedVariables { get; set; }

        /// <summary>
        /// Optional memory reference to a location appropriate for this result.
        /// </summary>
        /// <remarks>
        /// For pointer type eval results, this is generally a reference to the memory address contained in the 
        /// pointer. This attribute should be returned by a debug adapter if the client has passed the value true
        /// for the 'supportsMemoryReferences' capability of the 'initialize' request.
        /// </remarks>
        public string MemoryReference { get; set; }
    }
}
