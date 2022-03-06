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
