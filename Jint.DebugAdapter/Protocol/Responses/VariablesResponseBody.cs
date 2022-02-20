using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.DebugAdapter.Protocol.Types;

namespace Jint.DebugAdapter.Protocol.Responses
{
    internal class VariablesResponseBody : ProtocolResponseBody
    {
        public List<Variable> Variables { get; set; }
    }
}
