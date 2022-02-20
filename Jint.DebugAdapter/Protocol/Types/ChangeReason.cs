using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Types
{
    // Breakpoint event: reason: 'changed' | 'new' | 'removed' | string;
    // LoadedSource event: reason: 'new' | 'changed' | 'removed';
    // Module event: reason: 'new' | 'changed' | 'removed';
    internal enum ChangeReason
    {
        Other,
        New,
        Changed,
        Removed
    }
}
