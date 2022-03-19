using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter
{
    public interface IPauseHandler
    {
        void Pause();
        void Resume();
    }
}
