using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol
{
    public static class EventNames
    {
        public const string Breakpoint = "breakpoint";
        public const string Capabilities = "capabilities";
        public const string Continued = "continued";
        public const string Exited = "exited";
        public const string Initialized = "initialized";
        public const string Invalidated = "invalidated";
        public const string LoadedSource = "loadedSource";
        public const string Memory = "memory";
        public const string Module = "module";
        public const string Output = "output";
        public const string Process = "process";
        public const string ProgressEnd = "progressEnd";
        public const string ProgressStart = "progressStart";
        public const string ProgressUpdate = "progressUpdate";
        public const string Stopped = "stopped";
        public const string Terminated = "terminated";
        public const string Thread = "thread";
    }

    public static class CommandNames
    {
        public const string Disconnect = "disconnect";
        public const string ConfigurationDone = "configurationDone";
        public const string LoadedSources = "loadedSources";
        public const string Threads = "threads";
    }
}
