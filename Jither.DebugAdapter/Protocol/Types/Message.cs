using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A structured message object. Used to return errors from requests.
    /// </summary>
    public class Message
    {
        /// <param name="id">Unique identifier for the message.</param>
        /// <param name="format">A format string for the message.</param>
        [JsonConstructor]
        public Message(int id, string format)
        {
            Id = id;
            Format = format;
        }

        /// <summary>
        /// Unique identifier for the message.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A format string for the message.
        /// </summary>
        /// <remarks>
        /// Embedded variables have the form '{name}'. If variable name starts with an underscore character,
        /// the variable does not contain user data(PII) and can be safely used for telemetry purposes.
        /// </remarks>
        public string Format { get; set; }

        /// <summary>
        /// An object used as a dictionary for looking up the variables in the format string.
        /// </summary>
        public Dictionary<string, string> Variables { get; set; }

        /// <summary>
        /// If true, send to telemetry.
        /// </summary>
        public bool? SendTelemetry { get; set; }

        /// <summary>
        /// If true, show user.
        /// </summary>
        public bool? ShowUser { get; set; }

        /// <summary>
        /// An optional url where additional information about this message can be found.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// An optional label that is presented to the user as the UI for opening the url.
        /// </summary>
        public string UrlLabel { get; set; }
    }
}
