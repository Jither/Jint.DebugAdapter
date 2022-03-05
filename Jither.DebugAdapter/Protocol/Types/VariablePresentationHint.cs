namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// Optional properties of a variable that can be used to determine how to render the variable in the UI.
    /// </summary>
    public class VariablePresentationHint
    {
        /// <summary>
        /// The kind of variable. Before introducing additional values, try to use the listed values.
        /// </summary>
        public VariableKind Kind { get; set; }

        /// <summary>
        /// Set of attributes represented as an array of strings. Before introducing additional values, try to use
        /// the listed values.
        /// </summary>
        // TODO: List<StringEnum<>>
        public List<string> Attributes { get; set; }

        /// <summary>
        /// Visibility of variable. Before introducing additional values, try to use the listed values.
        /// </summary>
        public VariableVisibility Visibility { get; set; }

        /// <summary>
        /// If true, clients can present the variable with a UI that supports a specific gesture to trigger
        /// its evaluation.
        /// </summary>
        /// <remarks>
        /// This mechanism can be used for properties that require executing code when retrieving their value an
        /// where the code execution can be expensive and/or produce side-effects.A typical example are properties
        /// based on a getter function.
        /// 
        /// Please note that in addition to the 'lazy' flag, the variable's 'variablesReference' must refer to a 
        /// variable that will provide the value through another 'variable' request.
        /// </remarks>
        public bool? Lazy { get; set; }
    }
}
