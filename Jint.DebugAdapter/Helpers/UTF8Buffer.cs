using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jint.DebugAdapter.Helpers
{
    // TODO: Memory optimization
    internal class UTF8Buffer
    {
        private byte[] buffer;
        private readonly Encoding encoding = new UTF8Encoding(false, false);

        public UTF8Buffer()
        {
            buffer = Array.Empty<byte>();
        }

        public int ByteLength => this.buffer.Length;

        public void Append(byte[] newData, int length)
        {
            byte[] newBuffer = new byte[ByteLength + length];
            Array.Copy(buffer, 0, newBuffer, 0, ByteLength);
            Array.Copy(newData, 0, newBuffer, ByteLength, length);
            buffer = newBuffer;
        }

        public void Remove(int length)
        {
            byte[] newBuffer = new byte[ByteLength - length];
            Array.Copy(buffer, length, newBuffer, 0, ByteLength - length);
            buffer = newBuffer;
        }

        public string Peek() => encoding.GetString(buffer);

        public string Pop(int length)
        {
            string result = encoding.GetString(buffer, 0, length);
            Remove(length);
            return result;
        }
    }
}
