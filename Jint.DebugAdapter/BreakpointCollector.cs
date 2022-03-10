using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Esprima;
using Esprima.Ast;
using Esprima.Utils;

namespace Jint.DebugAdapter
{
    public enum BreakpointPositionType
    {
        None,
        Statement,
        Expression,
        Return
    }

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

    public class BreakpointCollector : AstVisitor
    {
        private readonly List<BreakpointPosition> positions = new();

        public List<BreakpointPosition> Positions => positions;

        public BreakpointCollector()
        {
        }

        public override void Visit(Node node)
        {
            if (node is Statement && node is not BlockStatement)
            {
                AddLocation(BreakpointPositionType.Statement, node.Location.Start);
            }
            base.Visit(node);
        }

        protected override void VisitDoWhileStatement(DoWhileStatement doWhileStatement)
        {
            base.VisitDoWhileStatement(doWhileStatement);

            AddLocation(BreakpointPositionType.Expression, doWhileStatement.Test.Location.Start);
        }

        protected override void VisitForInStatement(ForInStatement forInStatement)
        {
            base.VisitForInStatement(forInStatement);

            AddLocation(BreakpointPositionType.Expression, forInStatement.Left.Location.Start);
        }

        protected override void VisitForOfStatement(ForOfStatement forOfStatement)
        {
            base.VisitForOfStatement(forOfStatement);

            AddLocation(BreakpointPositionType.Expression, forOfStatement.Left.Location.Start);
        }

        protected override void VisitForStatement(ForStatement forStatement)
        {
            base.VisitForStatement(forStatement);

            if (forStatement.Test != null)
            {
                AddLocation(BreakpointPositionType.Expression, forStatement.Test.Location.Start);
            }
            if (forStatement.Update != null)
            {
                AddLocation(BreakpointPositionType.Expression, forStatement.Test.Location.Start);
            }
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

