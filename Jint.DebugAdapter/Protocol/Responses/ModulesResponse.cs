using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    /// <summary>
    /// Response to ‘modules’ request.
    /// </summary>
    public class ModulesResponse : ProtocolResponseBody
    {
        /// <param name="modules">All modules or range of modules.</param>
        public ModulesResponse(List<Module> modules)
        {
            Modules = modules;
        }

        /// <summary>
        /// All modules or range of modules.
        /// </summary>
        public List<Module> Modules { get; set; }

        /// <summary>
        /// The total number of modules available.
        /// </summary>
        public int? TotalModules { get; set; }
    }
}
