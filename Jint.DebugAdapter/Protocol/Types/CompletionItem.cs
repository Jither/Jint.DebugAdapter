using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    public class CompletionItem
    {
        public string Label { get; set; }
        public string Text { get; set; }
        public string SortText { get; set; }
        public string Detail { get; set; }
        public StringEnum<CompletionItemType>? Type { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
        public int? SelectionStart { get; set; }
        public int? SelectionLength { get; set; }
    }
}
