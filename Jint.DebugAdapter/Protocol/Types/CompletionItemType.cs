using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="CompletionItemType"/>
    public class CompletionItemType : StringEnum<CompletionItemType>
    {
        public static readonly CompletionItemType Method = Create("method");
        public static readonly CompletionItemType Function = Create("function");
        public static readonly CompletionItemType Constructor = Create("constructor");
        public static readonly CompletionItemType Field = Create("field");
        public static readonly CompletionItemType Variable = Create("variable");
        public static readonly CompletionItemType Class = Create("class");
        public static readonly CompletionItemType Interface = Create("interface");
        public static readonly CompletionItemType Module = Create("module");
        public static readonly CompletionItemType Property = Create("property");
        public static readonly CompletionItemType Unit = Create("unit");
        public static readonly CompletionItemType Value = Create("value");
        public static readonly CompletionItemType Enum = Create("enum");
        public static readonly CompletionItemType Keyword = Create("keyword");
        public static readonly CompletionItemType Snippet = Create("snippet");
        public static readonly CompletionItemType Text = Create("text");
        public static readonly CompletionItemType Color = Create("color");
        public static readonly CompletionItemType File = Create("file");
        public static readonly CompletionItemType Reference = Create("reference");
        public static readonly CompletionItemType CustomColor = Create("customcolor");
    }
}
