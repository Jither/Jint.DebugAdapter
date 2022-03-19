using Esprima;
using Esprima.Ast;
using Jint.DebugAdapter.Breakpoints;

namespace Jint.DebugAdapter
{
    public class EsprimaPositionComparer : IComparer<Position>
    {
        public static readonly EsprimaPositionComparer Default = new();

        public int Compare(Position x, Position y)
        {
            if (x.Line != y.Line)
            {
                return x.Line - y.Line;
            }
            return x.Column - y.Column;
        }
    }

    public class ScriptInfo
    {
        private List<Position> breakpointPositions;

        public Script Ast { get; }
        public List<Position> BreakpointPositions => breakpointPositions ??= CollectBreakpointPositions();

        public ScriptInfo(Script ast)
        {
            Ast = ast;
        }

        public IEnumerable<Position> FindBreakpointPositionsInRange(Position start, Position end)
        {
            var positions = BreakpointPositions;

            int index = positions.BinarySearch(start, EsprimaPositionComparer.Default);

            if (index < 0)
            {
                // Get the first break after the location
                index = ~index;
            }

            while (index < positions.Count)
            {
                var position = positions[index++];
                // We know we're past the start of the range. If we're also past the end, break
                if (EsprimaPositionComparer.Default.Compare(position, end) > 0)
                {
                    break;
                }

                yield return position;
            }
        }

        public Position FindNearestBreakpointPosition(Position position)
        {
            var positions = BreakpointPositions;
            int index = positions.BinarySearch(position, EsprimaPositionComparer.Default);
            if (index < 0)
            {
                // Get the first break after the location
                index = ~index;
            }
            return positions[index];
        }

        private List<Position> CollectBreakpointPositions()
        {
            var collector = new BreakpointCollector();
            collector.Visit(Ast);
            // Some statements may be at the same location
            var list = collector.Positions.Distinct().ToList();
            // We need the list sorted (it's going to be used for binary search)
            list.Sort(EsprimaPositionComparer.Default);
            return list;
        }
    }
}
