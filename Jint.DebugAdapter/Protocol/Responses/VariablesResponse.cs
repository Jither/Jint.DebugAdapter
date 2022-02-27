using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘variables’ request.
    /// </summary>
    public class VariablesResponse : ProtocolResponseBody
    {
        /// <param name="variables">All (or a range) of variables for the given variable reference.</param>
        public VariablesResponse(List<Variable> variables)
        {
            Variables = variables;
        }

        /// <summary>
        /// All (or a range) of variables for the given variable reference.
        /// </summary>
        public List<Variable> Variables { get; set; }
    }
}
