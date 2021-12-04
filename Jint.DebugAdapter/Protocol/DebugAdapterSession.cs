using System.Text;

namespace Jint.DebugAdapter.Protocol
{
    public class DebugAdapterSession
    {
        private const string ContentLengthName = "Content-Length";

        private readonly Stream input;
        private readonly Stream output;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        public DebugAdapterSession(Stream input, Stream output)
        {
            this.input = input;
            this.output = output;
            // Protocol headers are ASCII, content is UTF-8. Hence, we can use UTF-8 reader for the entire message
            reader = new StreamReader(input, Encoding.UTF8);
            writer = new StreamWriter(output, Encoding.UTF8);
        }

        public void Start()
        {
            while (true)
            {
                var message = ReadMessage();
                WriteMessage(message);
            }
        }

        private ProtocolMessage ReadMessage()
        {
            Dictionary<string, string> headerFields = new();
            while (true)
            {
                // TODO: This accepts \r and \n, while the protocol actually requires \r\n
                var line = reader.ReadLine();
                if (line == null)
                {
                    continue;
                }
                if (line == string.Empty)
                {
                    // Done with header fields
                    break;
                }

                string[] pair = line.Split(": ", 2, StringSplitOptions.None);
                if (pair.Length != 2)
                {
                    throw new ProtocolException($"Expected header field (key/value separated by ': '), but got {line}");
                }
                headerFields.Add(pair[0], pair[1]);
            }

            if (!headerFields.TryGetValue(ContentLengthName, out string strContentLength))
            {
                throw new ProtocolException($"No {ContentLengthName} header field found.");
            }
            if (!int.TryParse(strContentLength, out int contentLength))
            {
                throw new ProtocolException($"{ContentLengthName} is not a valid integer.");
            }

            // Conent-Length is the length of the content part in *bytes*.
            byte[] buffer = new byte[contentLength];
            input.Read(buffer);

            var message = new ProtocolMessage(Encoding.UTF8.GetString(buffer));
            return message;
        }

        private void WriteMessage(ProtocolMessage message)
        {
            writer.Write($"{ContentLengthName}: {message.ContentLength}\r\n");
            writer.Write("\r\n");
            writer.Write(message.RawContent);
            writer.Flush();
        }
    }
}