using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// CompletionItems are the suggestions returned from the CompletionsRequest.
    /// </summary>
    public class CompletionItem
    {
        /// <param name="label">The label of this completion item. By default this is also the text that is
        /// inserted when selecting this completion.</param>
        /// <param name="text">If text is not falsy then it is inserted instead of the label.</param>
        [JsonConstructor]
        public CompletionItem(string label, string text = null)
        {
            Label = label;
            Text = text;
        }

        /// <summary>
        /// The label of this completion item. By default this is also the text that is inserted when selecting
        /// this completion.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// If text is not falsy then it is inserted instead of the label.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// A string that should be used when comparing this item with other items. When `falsy` the label is used.
        /// </summary>
        public string SortText { get; set; }

        /// <summary>
        /// A human-readable string with additional information about this item, like type or symbol information.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// The item's type. Typically the client uses this information to render the item in the UI with an icon.
        /// </summary>
        public CompletionItemType Type { get; set; }

        /// <summary>
        /// This value determines the location (in the CompletionsRequest's 'text' attribute) where the completion 
        /// text is added. If missing the text is added at the location specified by the CompletionsRequest's 
        /// 'column' attribute.
        /// </summary>
        public int? Start { get; set; }

        /// <summary>
        /// This value determines how many characters are overwritten by the completion text.
        /// If missing the value 0 is assumed which results in the completion text being inserted.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Determines the start of the new selection after the text has been inserted (or replaced).
        /// The start position must in the range 0 and length of the completion text. If omitted the selection starts
        /// at the end of the completion text.
        /// </summary>
        public int? SelectionStart { get; set; }

        /// <summary>
        /// Determines the length of the new selection after the text has been inserted (or replaced).
        /// The selection can not extend beyond the bounds of the completion text. If omitted the length is assumed
        /// to be 0.
        /// </summary>
        public int? SelectionLength { get; set; }
    }
}
