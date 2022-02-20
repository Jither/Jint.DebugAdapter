using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    internal class SetExceptionBreakpointsArguments : ProtocolArguments
    {
        public List<string> Filters { get; set; }
        public List<ExceptionFilterOptions> FilterOptions { get; set; }
        public List<ExceptionOptions> ExceptionOptions { get; set; }
    }
}
