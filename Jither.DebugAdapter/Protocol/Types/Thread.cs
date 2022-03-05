using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A Thread
    /// </summary>
    public class Thread
    {
        /// <param name="id">Unique identifier for the thread.</param>
        /// <param name="name">A name of the thread.</param>
        [JsonConstructor]
        public Thread(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Unique identifier for the thread.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A name of the thread.
        /// </summary>
        public string Name { get; set; }
    }
}
