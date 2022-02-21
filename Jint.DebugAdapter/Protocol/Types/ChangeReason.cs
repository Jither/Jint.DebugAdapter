namespace Jint.DebugAdapter.Protocol.Types
{
    // Breakpoint event: reason: 'changed' | 'new' | 'removed' | string;
    // LoadedSource event: reason: 'new' | 'changed' | 'removed';
    // Module event: reason: 'new' | 'changed' | 'removed';
    public enum ChangeReason
    {
        Other,
        New,
        Changed,
        Removed
    }
}
