using System.Text.Json.Serialization;

namespace Jither.DebugAdapter.Protocol.Types
{
    /// <summary>
    /// A StepInTarget can be used in the ‘stepIn’ request and determines into which single target the stepIn
    /// request should step.
    /// </summary>
    public class StepInTarget
    {
        /// <param name="id">Unique identifier for a stepIn target.</param>
        /// <param name="label">The name of the stepIn target (shown in the UI).</param>
        [JsonConstructor]
        public StepInTarget(int id, string label)
        {
            Id = id;
            Label = label;
        }

        /// <summary>
        /// Unique identifier for a stepIn target.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the stepIn target (shown in the UI).
        /// </summary>
        public string Label { get; set; }
    }
}
