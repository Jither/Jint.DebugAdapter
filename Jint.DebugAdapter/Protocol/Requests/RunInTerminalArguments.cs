﻿using Jint.DebugAdapter.Helpers;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Requests
{
    public class RunInTerminalArguments : ProtocolArguments
    {
        public StringEnum<TerminalKind>? Kind { get; set; }
        public string Title { get; set; }
        public string Cwd { get; set; }
        public List<string> Args { get; set; }
        public Dictionary<string, string> Env { get; set; }
    }
}