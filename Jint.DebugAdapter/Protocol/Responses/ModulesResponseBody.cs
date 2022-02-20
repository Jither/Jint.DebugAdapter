using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class ModulesResponseBody : ProtocolResponseBody
    {
        public List<Module> Modules { get; set; }
        public int? TotalModules { get; set; }
    }
}
