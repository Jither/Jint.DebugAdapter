using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol
{
    public static class CommandNames
    {
        public const string Disconnect = "disconnect";
        public const string ConfigurationDone = "configurationDone";
        public const string LoadedSources = "loadedSources";
        public const string Threads = "threads";
    }
}
