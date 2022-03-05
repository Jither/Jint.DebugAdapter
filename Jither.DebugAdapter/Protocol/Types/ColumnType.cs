using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="ColumnType"/>
    public class ColumnType : StringEnum<ColumnType>
    {
        public static readonly ColumnType String = Create("string");
        public static readonly ColumnType Number = Create("number");
        public static readonly ColumnType Boolean = Create("boolean");
        public static readonly ColumnType UnixTimestampUTC = Create("unixTimestampUTC");
    }
}
