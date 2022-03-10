using Esprima;
using Esprima.Ast;

namespace Jint.DebugAdapter
{
    public class ScriptInfo
    {
        private List<BreakpointPosition> breakpointPositions;

        public Script Ast { get; }
        public List<BreakpointPosition> BreakpointPositions => breakpointPositions ??= CollectBreakpointPositions();

        public ScriptInfo(Script ast)
        {
            Ast = ast;
        }

        public IEnumerable<Position> FindBreakpointPositionsInRange(Position start, Position end)
        {
            var positions = BreakpointPositions;

            int index = positions.BinarySearch(new BreakpointPosition(BreakpointPositionType.None, start));

            if (index < 0)
            {
                // Get the first break after the location
                index = ~index;
            }

            while (index < positions.Count)
            {
                var position = positions[index++];
                // We know we're past the start of the range. If we're also past the end, break
                if (position.CompareTo(end) > 0)
                {
                    break;
                }
                
                yield return position.Position;
            }
        }

        public Position FindNearestBreakpointPosition(Position position)
        {
            var positions = BreakpointPositions;
            int index = positions.BinarySearch(new BreakpointPosition(BreakpointPositionType.None, position));
            if (index < 0)
            {
                // Get the first break after the location
                index = ~index;
            }
            return positions[index].Position;
        }

        private List<BreakpointPosition> CollectBreakpointPositions()
        {
            var collector = new BreakpointCollector();
            collector.Visit(Ast);
            // Some statements may be at the same location
            return collector.Positions.Distinct().ToList();
        }
    }
}
