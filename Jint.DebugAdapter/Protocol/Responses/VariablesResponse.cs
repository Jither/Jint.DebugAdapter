using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class VariablesResponse : ProtocolResponseBody
    {
        public List<Variable> Variables { get; set; }
    }
}
