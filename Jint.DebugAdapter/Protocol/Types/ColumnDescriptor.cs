using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal class ColumnDescriptor
    {
        public string AttributeName { get; set; }
        public string Label { get; set; }
        public string Format { get; set; }
        public StringEnum<ColumnType>? Type { get; set; }
        public int? Width { get; set; }
    }
}
