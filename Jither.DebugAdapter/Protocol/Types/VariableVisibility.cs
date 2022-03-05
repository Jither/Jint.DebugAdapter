using Jither.DebugAdapter.Helpers;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="VariableVisibility"/>
    public class VariableVisibility : StringEnum<VariableVisibility>
    {
        public static readonly VariableVisibility Public = Create("public");
        public static readonly VariableVisibility Private = Create("private");
        public static readonly VariableVisibility Protected = Create("protected");
        public static readonly VariableVisibility Internal = Create("internal");
        public static readonly VariableVisibility Final = Create("final");
    }
}
