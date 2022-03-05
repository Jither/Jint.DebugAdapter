using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jither.DebugAdapter.Helpers
{
    // Temporary facade for logging with no dependencies.

    [InterpolatedStringHandler]
    public ref struct LoggerStringHandler
    {
        private readonly StringBuilder builder;

        public LoggerStringHandler(int literalLength, int formattedCount, LogLevel level, out bool shouldAppend)
        {
            shouldAppend = level >= LogManager.Level;
            if (!shouldAppend)
            {
                builder = null;
                return;
            }
            builder = new StringBuilder(literalLength);
        }

        public LoggerStringHandler(int literalLength, int formattedCount)
        {
            builder = new StringBuilder(literalLength);
        }

        public void AppendLiteral(string str)
        {
            builder.Append(str);
        }

        public void AppendFormatted<T>(T value)
        {
            builder.Append(value?.ToString());
        }

        internal string GetFormattedText() => builder.ToString();
    }

    public enum LogLevel
    {
        Verbose,
        Info,
        Warning,
        Error,
        Quiet
    }

    public interface ILogProvider
    {
        void Log(LogLevel level, string message);
    }

    public class NullLogProvider : ILogProvider
    {
        public void Log(LogLevel level, string message)
        {
            
        }
    }

    public static class LogManager
    {
        public static LogLevel Level { get; set; } = LogLevel.Quiet;
        public static ILogProvider Provider { get; set; } = new NullLogProvider();

        public static Logger GetLogger()
        {
            return new Logger();
        }

        public static void Log(LogLevel level, LoggerStringHandler message)
        {
            if (level >= Level && Provider != null)
            {
                Provider.Log(level, message.GetFormattedText());
            }
        }

        public static void Log(LogLevel level, string message)
        {
            if (level >= Level && Provider != null)
            {
                Provider.Log(level, message);
            }
        }
    }

    public class Logger
    {
        public Logger()
        {
        }

        // Unfortunately, only this form will allow lazy evaluation of values in interpolated string
        public void Log(LogLevel level, [InterpolatedStringHandlerArgument("level")] LoggerStringHandler message)
        {
            LogManager.Log(level, message);
        }

        public void Info(LoggerStringHandler message)
        {
            LogManager.Log(LogLevel.Info, message);
        }

        public void Info(string message)
        {
            LogManager.Log(LogLevel.Info, message);
        }

        public void Verbose(LoggerStringHandler message)
        {
            LogManager.Log(LogLevel.Verbose, message);
        }

        public void Verbose(string message)
        {
            LogManager.Log(LogLevel.Verbose, message);
        }

        public void Warning(string message)
        {
            LogManager.Log(LogLevel.Warning, message);
        }

        public void Error(LoggerStringHandler message)
        {
            LogManager.Log(LogLevel.Error, message);
        }

        public void Error(string message)
        {
            LogManager.Log(LogLevel.Error, message);
        }
    }
}
