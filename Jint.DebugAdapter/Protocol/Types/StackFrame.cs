using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class StackFrame
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Source Source { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public int? EndLine { get; set; }
        public int? EndColumn { get; set; }
        public bool? CanRestart { get; set; }
        public string InstructionPointerReference { get; set; }
        public string ModuleId { get; set; }
        public string PresentationHint { get; set; }
    }
}
