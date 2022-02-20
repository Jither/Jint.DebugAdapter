using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    internal class ProcessEventBody : ProtocolEventBody
    {
        public string Name { get; set; }
        public int? SystemProcessId { get; set; }
        public bool? IsLocalProcess { get; set; }
        public StartMethod? StartMethod { get; set; }
        public int? PointerSize { get; set; }
    }
}
