using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    /// <completionlist cref="TerminalKind"/>
    public class TerminalKind : StringEnum<TerminalKind>
    {
        public static readonly TerminalKind Integrated = Create("integrated");
        public static readonly TerminalKind External = Create("external");
    }
}
