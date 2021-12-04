using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol
{
    internal class ProtocolMessage
    {
        public int ContentLength => Encoding.UTF8.GetByteCount(RawContent);
        public string RawContent { get; }

        public ProtocolMessage(string rawContent)
        {
            RawContent = rawContent;
        }
    }

}
