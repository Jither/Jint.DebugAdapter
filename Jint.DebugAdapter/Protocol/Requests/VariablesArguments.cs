using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Retrieves all child variables for the given variable reference.
    /// </summary>
    /// <remarks>
    /// An optional filter can be used to limit the fetched children to either named or indexed children.
    /// </remarks>
    public class VariablesArguments : ProtocolArguments
    {
        /// <summary>
        /// The Variable reference.
        /// </summary>
        public int VariablesReference { get; set; }

        /// <summary>
        /// Optional filter to limit the child variables to either named or indexed. If omitted, 
        /// both types are fetched.
        /// </summary>
        public StringEnum<VariableFilter>? Filter { get; set; }

        /// <summary>
        /// The index of the first variable to return; if omitted children start at 0.
        /// </summary>
        public int? Start { get; set; }

        /// <summary>
        /// The number of variables to return. If count is missing or 0, all variables are returned.
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Specifies details on how to format the Variable values. The attribute is only honored by a debug adapter
        /// if the capability 'supportsValueFormattingOptions' is true.
        /// </summary>
        public ValueFormat Format { get; set; }
    }
}
