using System.Diagnostics;

namespace BloodShadow.Core.Logger
{
    public abstract class Logger
    {
        public static Logger Instance
        {
            get => _instance;
            set { if (value != null) { _instance = value; } }
        }

        private static Logger _instance = new ConsoleLogger();
        public static void WriteLineFatal(LoggerLabel label, string message, StackTrace stackTrace, Exception exception) { WriteLine(MessageChanel.FATAL, label, message, stackTrace, exception); }
        public static void WriteLineFatal(LoggerLabel label, string message, Exception exception) { WriteLineFatal(label, message, new(1), exception); }
        public static void WriteLineException(LoggerLabel label, string message, StackTrace stackTrace, Exception exception) { WriteLine(MessageChanel.ERROR, label, message, stackTrace, exception); }
        public static void WriteLineException(LoggerLabel label, string message, Exception exception) { WriteLineException(label, message, new(1), exception); }
        public static void WriteLineWarning(LoggerLabel label, string message, StackTrace stackTrace) { WriteLine(MessageChanel.WARN, label, message, stackTrace, null); }
        public static void WriteLineWarning(LoggerLabel label, string message) { WriteLineWarning(label, message, new(1)); }
        public static void WriteLineDebug(LoggerLabel label, string message) { WriteLine(MessageChanel.DEBUG, label, message, null, null); }
        public static void WriteLineInfo(LoggerLabel label, string message) { WriteLine(MessageChanel.INFO, label, message, null, null); }
        public static void WriteFatal(LoggerLabel label, string message, StackTrace stackTrace, Exception? exception) { Write(MessageChanel.FATAL, label, message, stackTrace, exception); }
        public static void WriteException(LoggerLabel label, string message, StackTrace stackTrace, Exception? exception) { Write(MessageChanel.ERROR, label, message, stackTrace, exception); }
        public static void WriteWarning(LoggerLabel label, string message, StackTrace stackTrace) { Write(MessageChanel.WARN, label, message, stackTrace, null); }
        public static void WriteDebug(LoggerLabel label, string message) { Write(MessageChanel.DEBUG, label, message, null, null); }
        public static void WriteInfo(LoggerLabel label, string message) { Write(MessageChanel.INFO, label, message, null, null); }
        public static void WriteLine(MessageChanel chanel, LoggerLabel label, string message, StackTrace? stackTrace, Exception? exception) => Write(chanel, label, $"{message}\n", stackTrace, exception);
        public static void Write(MessageChanel chanel, LoggerLabel label, string message, StackTrace? stackTrace, Exception? exception) => _instance.WriteInternal(chanel, label, message, stackTrace, exception);
        protected abstract void WriteInternal(MessageChanel chanel, LoggerLabel label, string message, StackTrace? stackTrace, Exception? exception);
    }
}
