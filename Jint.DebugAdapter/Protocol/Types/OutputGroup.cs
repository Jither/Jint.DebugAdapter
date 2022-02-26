namespace Jint.DebugAdapter.Protocol.Types
{
    public enum OutputGroup
    {
        Other,
        /// <summary>
        /// Start a new group in expanded mode. Subsequent output events are members of the group and should be shown indented.
        /// </summary>
        /// <remarks>
        /// The 'output' attribute becomes the name of the group and is not indented.
        /// </remarks>
        Start,

        /// <summary>
        /// Start a new group in collapsed mode.
        /// </summary>
        /// <remarks>
        /// Subsequent output events are members of the group and should be shown indented (as soon as the group is expanded).
        /// The 'output' attribute becomes the name of the group and is not indented.
        /// </remarks>
        StartCollapsed,

        /// <summary>
        /// End the current group and decreases the indentation of subsequent output events.
        /// </summary>
        /// <remarks>
        /// A non empty 'output' attribute is shown as the unindented end of the group.
        /// </remarks>
        End
    }
}
