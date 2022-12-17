using System.Text.RegularExpressions;
using System.Web;
using Esprima;
using Esprima.Ast;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter.BreakPoints
{
    /// <summary>
    /// Custom breakpoint extensions for this debugger implementation. Adds hit conditions and log points.
    /// </summary>
    public class ExtendedBreakPoint : BreakPoint
    {
        private static readonly Regex rxHitCondition = new(@"^((?<operator>(?:<=?|>=?|={1,3}|%))\s*)?(?<count>\d+)$");

        public ExtendedBreakPoint(string source, int line, int column, string condition = null, string hitCondition = null, string logMessage = null)
            : base(source, line, column, condition)
        {
            HitCondition = ParseHitCondition(hitCondition);
            LogMessage = LogMessageToAst(logMessage);
        }

        public Func<uint, bool> HitCondition { get; set; }
        public uint HitCount { get; set; }
        public Script LogMessage { get; set; }

        private Func<uint, bool> ParseHitCondition(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                return null;
            }
            var match = rxHitCondition.Match(condition);
            if (!match.Success)
            {
                throw new FormatException($"Invalid hit condition: {condition}");
            }

            string op = match.Groups["operator"].Success ? match.Groups["operator"].Value : "=";
            string strCount = match.Groups["count"].Value;

            if (!int.TryParse(strCount, out int count))
            {
                throw new FormatException($"Invalid hit condition: {condition} - count should be a 32 bit integer");
            }

            return op switch
            {
                "=" or "==" or "===" => c => c == count,
                "<" => c => c < count,
                "<=" => c => c <= count,
                ">" => c => c > count,
                ">=" => c => c >= count,
                "%" => c => c % count == 0,
                _ => throw new NotImplementedException($"Cannot parse hit condition operator '{op}'")
            };
        }

        private Script LogMessageToAst(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return null;
            }

            var parts = new List<Expression>();
            int end = 0;

            void AddLiteral(string value)
            {
                value = HttpUtility.JavaScriptStringEncode(value);
                parts.Add(new Literal(value, "\"" + value + "\""));
            }

            // Build a list of string literals (outside braces) and parsed expressions (inside braces):
            while (true)
            {
                int start = message.IndexOf('{', end);
                if (start < 0)
                {
                    AddLiteral(message[end..]);
                    break;
                }
                AddLiteral(message[end..start]);

                var parser = new JavaScriptParser();
                Script partAst;
                try
                {
                    partAst = parser.ParseScript(message[start..]);
                }
                catch (ParserException ex)
                {
                    throw new FormatException($"Invalid log point code: {ex.Message}");
                }
                end = start + partAst.Range.End;

                string code = message[start..end];

                // If braces were empty or unclosed, treat as string literal
                if (end - 1 == start + 1 || message[end - 1] != '}')
                {
                    AddLiteral(code);
                    continue;
                }

                if (partAst.Body[0] is not BlockStatement block)
                {
                    throw new FormatException($"Invalid log point code: {code} - not a block");
                }

                if (block.Body.Count != 1 || block.Body[0] is not ExpressionStatement exprStmt)
                {
                    throw new FormatException($"Invalid log point code: {code} - not a valid expression.");
                }

                parts.Add(exprStmt.Expression);
            }

            // Combine our parts into a single Script AST:
            Expression expr = parts[^1];
            for (int i = parts.Count - 2; i >= 0; i--)
            {
                expr = new BinaryExpression("+", parts[i], expr);
            }
            var statement = new ExpressionStatement(expr);

            return new Script(NodeList.Create<Statement>(new[] { statement }), strict: true);
        }
    }
}
