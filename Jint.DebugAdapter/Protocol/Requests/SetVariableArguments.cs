using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    /// <summary>
    /// Set the variable with the given name in the variable container to a new value.
    /// </summary>
    /// <remarks>
    /// Clients should only call this request if the capability ‘supportsSetVariable’ is true.
    /// If a debug adapter implements both setVariable and setExpression, a client will only use setExpression if
    /// the variable has an evaluateName property.
    /// </summary>
    public class SetVariableArguments : ProtocolArguments
    {
        /// <summary>
        /// The reference of the variable container.
        /// </summary>
        public int VariablesReference { get; set; }

        /// <summary>
        /// The name of the variable in the container.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the variable.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Specifies details on how to format the response value.
        /// </summary>
        public ValueFormat Format { get; set; }
    }
}
