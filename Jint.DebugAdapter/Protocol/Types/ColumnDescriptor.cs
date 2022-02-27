using System.Text.Json.Serialization;
using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A ColumnDescriptor specifies what module attribute to show in a column of the ModulesView, how to format it,
    /// and what the column’s label should be.
    /// </summary>
    /// <remarks>
    /// It is only used if the underlying UI actually supports this level of customization.
    /// </remarks>
    public class ColumnDescriptor
    {
        /// <param name="attributeName">Name of the attribute rendered in this column.</param>
        /// <param name="label">Header UI label of column.</param>
        [JsonConstructor]
        public ColumnDescriptor(string attributeName, string label)
        {
            AttributeName = attributeName;
            Label = label;
        }

        /// <summary>
        /// Name of the attribute rendered in this column.
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Header UI label of column.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Format to use for the rendered values in this column. TBD how the format strings looks like.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Datatype of values in this column.  Defaults to 'string' if not specified.
        /// </summary>
        public ColumnType Type { get; set; }

        /// <summary>
        /// Width of this column in characters (hint only).
        /// </summary>
        public int? Width { get; set; }
    }
}
