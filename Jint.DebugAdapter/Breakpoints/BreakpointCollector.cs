using Esprima;
using Esprima.Ast;
using Esprima.Utils;

namespace Jint.DebugAdapter.BreakPoints
{
    public class BreakPointCollector : AstVisitor
    {
        private readonly List<Position> positions = new();

        public List<Position> Positions => positions;

        public BreakPointCollector()
        {
        }

        public override object Visit(Node node)
        {
            if (node is Statement && node is not BlockStatement)
            {
                positions.Add(node.Location.Start);
            }
            base.Visit(node);
            
            return node;
        }

        protected override object VisitDoWhileStatement(DoWhileStatement doWhileStatement)
        {
            base.VisitDoWhileStatement(doWhileStatement);

            positions.Add(doWhileStatement.Test.Location.Start);

            return doWhileStatement;
        }

        protected override object VisitForInStatement(ForInStatement forInStatement)
        {
            base.VisitForInStatement(forInStatement);

            positions.Add(forInStatement.Left.Location.Start);

            return forInStatement;
        }

        protected override object VisitForOfStatement(ForOfStatement forOfStatement)
        {
            base.VisitForOfStatement(forOfStatement);

            positions.Add(forOfStatement.Left.Location.Start);

            return forOfStatement;
        }

        protected override object VisitForStatement(ForStatement forStatement)
        {
            base.VisitForStatement(forStatement);

            if (forStatement.Test != null)
            {
                positions.Add(forStatement.Test.Location.Start);
            }
            if (forStatement.Update != null)
            {
                positions.Add(forStatement.Update.Location.Start);
            }

            return forStatement;
        }

        protected override object VisitArrowFunctionExpression(ArrowFunctionExpression arrowFunctionExpression)
        {
            base.VisitArrowFunctionExpression(arrowFunctionExpression);

            positions.Add(arrowFunctionExpression.Body.Location.End);

            return arrowFunctionExpression;
        }

        protected override object VisitFunctionDeclaration(FunctionDeclaration functionDeclaration)
        {
            base.VisitFunctionDeclaration(functionDeclaration);

            positions.Add(functionDeclaration.Body.Location.End);

            return functionDeclaration;
        }

        protected override object VisitFunctionExpression(FunctionExpression function)
        {
            base.VisitFunctionExpression(function);

            positions.Add(function.Body.Location.End);

            return function;
        }
    }
}

