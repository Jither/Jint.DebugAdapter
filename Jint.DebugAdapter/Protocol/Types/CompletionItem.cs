using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class CompletionItem
    {
        public string Label { get; set; }
        public string Text { get; set; }
        public string SortText { get; set; }
        public string Detail { get; set; }
        public CompletionItemType? Type { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
        public int? SelectionStart { get; set; }
        public int? SelectionLength { get; set; }
    }
}
