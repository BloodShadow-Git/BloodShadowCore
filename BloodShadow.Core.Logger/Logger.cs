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
        public static int MinLoggerLevel = 0;

        private static Logger _instance = new ConsoleLogger();
        public static void WriteLine(MessageChanel chanel, LoggerLabel label, string template, StackTrace? stackTrace, Exception? exception, params object[] datas) => Write(chanel, label, $"{template}\n", stackTrace, exception, datas);
        public static void Write(MessageChanel chanel, LoggerLabel label, string template, StackTrace? stackTrace, Exception? exception, params object[] datas) { if (chanel.Level >= MinLoggerLevel) { _instance.WriteInternal(chanel, label, template, stackTrace, exception, datas); } }
        protected abstract void WriteInternal(MessageChanel chanel, LoggerLabel label, string template, StackTrace? stackTrace, Exception? exception, params object[] datas);
    }
}
