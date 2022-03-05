using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Esprima.Ast;
using Esprima.Utils;

namespace Jint.DebugAdapter
{
    public enum BreakpointPositionType
    {
        None,
        Statement,
        Return
    }

    public record BreakpointPosition : IComparable<BreakpointPosition>, IEquatable<BreakpointPosition>
    {
        public BreakpointPositionType Type { get; }
        public Esprima.Position Position { get; }

        public BreakpointPosition(BreakpointPositionType type, Esprima.Position position)
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
    }

    public class BreakpointCollector : AstVisitor
    {
        private readonly List<BreakpointPosition> positions = new();

        public List<BreakpointPosition> Positions => positions;

        public BreakpointCollector()
        {
        }

        public override void Visit(Node node)
        {
            if (node is Statement)
            {
                AddLocation(BreakpointPositionType.Statement, node.Location.Start);
            }
            base.Visit(node);
        }

        protected override void VisitArrowFunctionExpression(ArrowFunctionExpression arrowFunctionExpression)
        {
            base.VisitArrowFunctionExpression(arrowFunctionExpression);

            AddLocation(BreakpointPositionType.Return, arrowFunctionExpression.Body.Location.End);
        }

        protected override void VisitFunctionDeclaration(FunctionDeclaration functionDeclaration)
        {
            base.VisitFunctionDeclaration(functionDeclaration);

            AddLocation(BreakpointPositionType.Return, functionDeclaration.Body.Location.End);
        }

        protected override void VisitFunctionExpression(IFunction function)
        {
            base.VisitFunctionExpression(function);

            AddLocation(BreakpointPositionType.Return, function.Body.Location.End);
        }

        private void AddLocation(BreakpointPositionType type, Esprima.Position position)
        {
            var location = new BreakpointPosition(type, position);
            positions.Add(location);
        }
    }
}

