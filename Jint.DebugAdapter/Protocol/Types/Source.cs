using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class Source
    {
        public string Name { get; set; }
        public int? SourceReference { get; set; }
        public string PresentationHint { get; set; }
        public string Origin { get; set; }
        public List<Source> Sources { get; set; }
        public object AdapterData { get; set; }
        public List<Checksum> Checksums { get; set; }
    }
}
