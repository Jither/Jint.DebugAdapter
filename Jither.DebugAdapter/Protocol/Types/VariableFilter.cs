using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="VariableFilter"/>
    public class VariableFilter : StringEnum<VariableFilter>
    {
        public static readonly VariableFilter Indexed = Create("indexed");
        public static readonly VariableFilter Named = Create("named");
    }
}
