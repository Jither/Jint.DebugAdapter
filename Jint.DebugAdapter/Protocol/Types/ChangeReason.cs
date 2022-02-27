using Jint.DebugAdapter.Helpers;

namespace Jint.DebugAdapter.Protocol.Types
{
    // Breakpoint event: reason: 'changed' | 'new' | 'removed' | string;
    // LoadedSource event: reason: 'new' | 'changed' | 'removed';
    // Module event: reason: 'new' | 'changed' | 'removed';
    /// <completionlist cref="ChangeReason"/>
    public class ChangeReason : StringEnum<ChangeReason>
    {
        public static readonly ChangeReason New = Create("new");
        public static readonly ChangeReason Changed = Create("changed");
        public static readonly ChangeReason Removed = Create("removed");
    }
}
