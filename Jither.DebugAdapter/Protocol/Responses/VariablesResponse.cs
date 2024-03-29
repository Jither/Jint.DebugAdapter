﻿using Jither.DebugAdapter.Protocol.Types;

namespace Jither.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘variables’ request.
    /// </summary>
    public class VariablesResponse : ProtocolResponseBody
    {
        /// <param name="variables">All (or a range) of variables for the given variable reference.</param>
        public VariablesResponse(IEnumerable<Variable> variables)
        {
            Variables = variables;
        }

        /// <summary>
        /// All (or a range) of variables for the given variable reference.
        /// </summary>
        public IEnumerable<Variable> Variables { get; set; }
    }
}
