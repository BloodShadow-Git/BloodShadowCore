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
        public static void WriteLine(MessageChanel chanel, LoggerLabel label, string message, StackTrace? stackTrace, Exception? exception) => Write(chanel, label, $"{message}\n", stackTrace, exception);
        public static void Write(MessageChanel chanel, LoggerLabel label, string message, StackTrace? stackTrace, Exception? exception) => _instance.WriteInternal(chanel, label, message, stackTrace, exception);
        protected abstract void WriteInternal(MessageChanel chanel, LoggerLabel label, string message, StackTrace? stackTrace, Exception? exception);
    }
}
