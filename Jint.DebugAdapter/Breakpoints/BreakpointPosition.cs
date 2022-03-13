using Esprima;

namespace Jint.DebugAdapter.Breakpoints
{
    // TODO: Consider just using Esprima.Position (needs to be IComparable and IEquatable) - do we actually need Type?
    public record BreakpointPosition : IComparable<BreakpointPosition>, IComparable<Position>, IEquatable<BreakpointPosition>
    {
        public BreakpointPositionType Type { get; }
        public Position Position { get; }

        public BreakpointPosition(BreakpointPositionType type, Position position)
        {
            Type = type;
            Position = position;
        }

        public int CompareTo(BreakpointPosition other)
        {
            if (Position.Line != other.Position.Line)
            {
                return Position.Line - other.Position.Line;
            }
            return Position.Column - other.Position.Column;
        }

        public int CompareTo(Position other)
        {
            if (Position.Line != other.Line)
            {
                return Position.Line - other.Line;
            }
            return Position.Column - other.Column;
        }
    }
}

