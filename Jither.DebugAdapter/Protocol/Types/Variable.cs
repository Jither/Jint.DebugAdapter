using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A Variable is a name/value pair.
    /// </summary>
    /// <remarks>
    /// Optionally a variable can have a ‘type’ that is shown if space permits or when hovering over the
    /// variable’s name.
    /// 
    /// An optional ‘kind’ is used to render additional properties of the variable, e.g.different icons can be used
    /// to indicate that a variable is public or private.
    /// 
    /// If the value is structured(has children), a handle is provided to retrieve the children with
    /// the VariablesRequest.
    /// 
    /// If the number of named or indexed children is large, the numbers should be returned via the optional
    /// ‘namedVariables’ and ‘indexedVariables’ attributes.
    /// 
    /// The client can use this optional information to present the children in a paged UI and fetch them in chunks.
    /// </remarks>
    public class Variable
    {
        /// <param name="name">The variable's name.</param>
        /// <param name="value">The variable's value</param>
        [JsonConstructor]
        public Variable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// The variable's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The variable's value.
        /// </summary>
        /// <remarks>
        /// This can be a multi-line text, e.g. for a function the body of a function.
        /// For structured variables(which do not have a simple value), it is recommended to provide a one line 
        /// representation of the structured object. This helps to identify the structured object in the collapsed
        /// state when its children are not yet visible. An empty string can be used if no value should be shown 
        /// in the UI.
        /// </remarks>
        public string Value { get; set; }

        /// <summary>
        /// The type of the variable's value.
        /// </summary>
        /// <remarks>
        /// Typically shown in the UI when hovering over the value. This attribute should only be returned by a debug
        /// adapter if the client has passed the value true for the 'supportsVariableType' capability of the
        /// 'initialize' request.
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Properties of a variable that can be used to determine how to render the variable in the UI.
        /// </summary>
        public VariablePresentationHint PresentationHint { get; set; }

        /// <summary>
        /// Optional evaluatable name of this variable which can be passed to the 'EvaluateRequest' to fetch the
        /// variable's value.
        /// </summary>
        public string EvaluateName { get; set; }

        /// <summary>
        /// If variablesReference is > 0, the variable is structured and its children can be retrieved by passing
        /// variablesReference to the VariablesRequest.
        /// </summary>
        public int VariablesReference { get; set; }

        /// <summary>
        /// The number of named child variables. The client can use this optional information to present the children
        /// in a paged UI and fetch them in chunks.
        /// </summary>
        public int? NamedVariables { get; set; }

        /// <summary>
        /// The number of indexed child variables. The client can use this optional information to present the
        /// children in a paged UI and fetch them in chunks.
        /// </summary>
        public int? IndexedVariables { get; set; }

        /// <summary>
        /// Optional memory reference for the variable if the variable represents executable code, such as a function
        /// pointer.
        /// </summary>
        /// <remarks>
        /// This attribute is only required if the client has passed the value true for the 'supportsMemoryReferences'
        /// capability of the 'initialize' request.
        /// </remarks>
        public string MemoryReference { get; set; }
    }
}
