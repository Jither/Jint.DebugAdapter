using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Protocol.Types
{
    internal enum StartMethod
    {
        Other,
        Launch,
        Attach,
        AttachForSuspendedLaunch
    }
}
