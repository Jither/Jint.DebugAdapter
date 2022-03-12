using System.Text.RegularExpressions;
using Jint.Runtime.Debugger;

namespace Jint.DebugAdapter
{
    /// <summary>
    /// Custom breakpoint extensions for this debugger implementation. Adds hit conditions and logpoints.
    /// </summary>
    public class ExtendedBreakPoint : BreakPoint
    {
        private static Regex rxHitCondition = new Regex(@"^((?<operator>(?:<=?|>=?|={1,3}|%))\s*)?(?<count>\d+)$");

        public ExtendedBreakPoint(string source, int line, int column, string condition = null, string hitCondition = null, string logMessage = null) 
            : base(source, line, column, condition)
        {
            HitCondition = ParseHitCondition(hitCondition);
            LogMessage = logMessage;
        }

        public Func<uint, bool> HitCondition { get; set; }
        public uint HitCount { get; set; }
        public string LogMessage { get; set; }

        private Func<uint, bool> ParseHitCondition(string condition)
        {
            if (String.IsNullOrEmpty(condition))
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

            if (!Int32.TryParse(strCount, out int count))
            {
                throw new FormatException($"Invalid hit condition: {condition} - count should be a 32 bit integer");
            }

            return op switch
            {
                "=" or "==" or "===" => (c => c == count),
                "<" => (c => c < count),
                "<=" => (c => c <= count),
                ">" => (c => c > count),
                ">=" => (c => c >= count),
                "%" => (c => (c % count) == 0),
                _ => throw new NotImplementedException($"Cannot parse hit condition operator '{op}'")
            };
        }
    }
}
