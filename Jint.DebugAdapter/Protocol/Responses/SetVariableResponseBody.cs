﻿namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class SetVariableResponseBody : ProtocolResponseBody
    {
        public string Value { get; set; }
        public string Type { get; set; }
        public int? VariablesReference { get; set; }
        public int? NamedVariables { get; set; }
        public int? IndexedVariables { get; set; }
    }
}
