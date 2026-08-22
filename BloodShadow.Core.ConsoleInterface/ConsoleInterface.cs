namespace BloodShadow.Core.ConsoleInterface
{
    public abstract class ConsoleInterface
    {
        public static ConsoleInterface Instance
        {
            get => _instance;
            set { if (value != null) { _instance = value; } }
        }

        private static ConsoleInterface _instance = new DefaultConsoleInterface();
        public static string? ReadLineException(object? mess) { return ReadLine(mess, MessageChanel.ERROR); }
        public static string? ReadLineWarning(object? mess) { return ReadLine(mess, MessageChanel.WARN); }
        public static string? ReadLineInfo(object? mess) { return ReadLine(mess, MessageChanel.INFO); }
        public static string? ReadLineException() { return ReadLine(MessageChanel.ERROR); }
        public static string? ReadLineWarning() { return ReadLine(MessageChanel.WARN); }
        public static string? ReadLineInfo() { return ReadLine(MessageChanel.INFO); }
        public static string? ReadException(object? mess) { return Read(mess, MessageChanel.ERROR); }
        public static string? ReadWarning(object? mess) { return Read(mess, MessageChanel.WARN); }
        public static string? ReadInfo(object? mess) { return Read(mess, MessageChanel.INFO); }
        public static void WriteLineException(object? mess) { WriteLine(mess, MessageChanel.ERROR); }
        public static void WriteLineWarning(object? mess) { WriteLine(mess, MessageChanel.WARN); }
        public static void WriteLineInfo(object? mess) { WriteLine(mess, MessageChanel.INFO); }
        public static void WriteException(object? mess) { Write(mess, MessageChanel.ERROR); }
        public static void WriteWarning(object? mess) { Write(mess, MessageChanel.WARN); }
        public static void WriteInfo(object? mess) { Write(mess, MessageChanel.INFO); }
        public static string? ReadLine(object? mess, MessageChanel chanel)
        {
            WriteLine(mess, chanel);
            return _instance.ReadInternal();
        }
        public static string? ReadLine(MessageChanel chanel) => ReadLine(null, chanel);
        public static string? Read(object? mess, MessageChanel chanel)
        {
            Write(mess, chanel);
            return _instance.ReadInternal();
        }
        public static void WriteLine(object? mess, MessageChanel chanel) => Write($"{mess}\n", chanel);
        public static void Write(object? mess, MessageChanel chanel) => _instance.WriteInternal(mess, chanel);
        protected abstract string? ReadInternal();
        protected abstract void WriteInternal(object? mess, MessageChanel chanel);
    }
}
