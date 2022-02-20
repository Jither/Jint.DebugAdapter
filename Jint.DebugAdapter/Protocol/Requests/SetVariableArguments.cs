using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class SetVariableArguments : ProtocolArguments
    {
        public int VariablesReference { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ValueFormat Format { get; set; }
    }
}
