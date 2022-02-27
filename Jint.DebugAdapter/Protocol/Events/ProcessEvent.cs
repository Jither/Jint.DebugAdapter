using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Events
{
    /// <summary>
    /// The event indicates that the debugger has begun debugging a new process. Either one that it has launched,
    /// or one that it has attached to.
    /// </summary>
    public class ProcessEvent : ProtocolEventBody
    {
        protected override string EventNameInternal => "process";

        public ProcessEvent(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The logical name of the process. This is usually the full path to process's executable file.
        /// Example: /home/example/myproj/program.js.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The system process id of the debugged process. This property will be missing for non-system processes.
        /// </summary>
        public int? SystemProcessId { get; set; }

        /// <summary>
        /// If true, the process is running on the same computer as the debug adapter.
        /// </summary>
        public bool? IsLocalProcess { get; set; }

        /// <summary>
        /// Describes how the debug engine started debugging this process.
        /// </summary>
        public StartMethod StartMethod { get; set; }

        /// <summary>
        /// The size of a pointer or address for this process, in bits.
        /// This value may be used by clients when formatting addresses for display.
        /// </summary>
        public int? PointerSize { get; set; }
    }
}
