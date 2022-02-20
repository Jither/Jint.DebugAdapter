using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter
{
    public static class Logger
    {
        static Logger()
        {
            File.Delete(@"D:\log.txt");
        }
        public static void Log(string message)
        {
            File.AppendAllText(@"D:\log.txt", message + Environment.NewLine);
            Console.WriteLine(message);
        }
    }
}
