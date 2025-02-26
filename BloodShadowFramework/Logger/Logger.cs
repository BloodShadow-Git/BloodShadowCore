namespace BloodShadowFramework.Logger
{
    public static class Logger
    {
        public const ConsoleColor ERROR_FORE_COLOR = ConsoleColor.Red;
        public const ConsoleColor ERROR_BACK_COLOR = ConsoleColor.Black;

        public const ConsoleColor WARNING_FORE_COLOR = ConsoleColor.Yellow;
        public const ConsoleColor WARNING_BACK_COLOR = ConsoleColor.Black;

        public const ConsoleColor MESSAGE_FORE_COLOR = ConsoleColor.White;
        public const ConsoleColor MESSAGE_BACK_COLOR = ConsoleColor.Black;

        public static string ReadLineError(object mess) { return ColorReadLine(mess, ERROR_FORE_COLOR, ERROR_BACK_COLOR); }
        public static string ReadLineWarning(object mess) { return ColorReadLine(mess, WARNING_FORE_COLOR, WARNING_BACK_COLOR); }
        public static string ReadLineMessage(object mess) { return ColorReadLine(mess, MESSAGE_FORE_COLOR, MESSAGE_BACK_COLOR); }

        public static string ReadLineError() { return ColorReadLine(ERROR_FORE_COLOR, ERROR_BACK_COLOR); }
        public static string ReadLineWarning() { return ColorReadLine(WARNING_FORE_COLOR, WARNING_BACK_COLOR); }
        public static string ReadLineMessage() { return ColorReadLine(MESSAGE_FORE_COLOR, MESSAGE_BACK_COLOR); }

        public static string ReadError(object mess) { return ColorRead(mess, ERROR_FORE_COLOR, ERROR_BACK_COLOR); }
        public static string ReadWarning(object mess) { return ColorRead(mess, WARNING_FORE_COLOR, WARNING_BACK_COLOR); }
        public static string ReadMessage(object mess) { return ColorRead(mess, MESSAGE_FORE_COLOR, MESSAGE_BACK_COLOR); }

        public static void WriteLineError(object mess) { ColorWriteLine(mess, ERROR_FORE_COLOR, ERROR_BACK_COLOR); }
        public static void WriteLineWarning(object mess) { ColorWriteLine(mess, WARNING_FORE_COLOR, WARNING_BACK_COLOR); }
        public static void WriteLineMessage(object mess) { ColorWriteLine(mess, MESSAGE_FORE_COLOR, MESSAGE_BACK_COLOR); }

        public static void WriteLineError() { ColorWriteLine(ERROR_FORE_COLOR, ERROR_BACK_COLOR); }
        public static void WriteLineWarning() { ColorWriteLine(WARNING_FORE_COLOR, WARNING_BACK_COLOR); }
        public static void WriteLineMessage() { ColorWriteLine(MESSAGE_FORE_COLOR, MESSAGE_BACK_COLOR); }

        public static void WriteError(object mess) { ColorWrite(mess, ERROR_FORE_COLOR, ERROR_BACK_COLOR); }
        public static void WriteWarning(object mess) { ColorWrite(mess, WARNING_FORE_COLOR, WARNING_BACK_COLOR); }
        public static void WriteMessage(object mess) { ColorWrite(mess, MESSAGE_FORE_COLOR, MESSAGE_BACK_COLOR); }

        public static string ColorReadLine(object mess, ConsoleColor BC, ConsoleColor FC)
        {
            ColorWriteLine(mess, BC, FC);
            return Console.ReadLine();
        }
        public static string ColorReadLine(ConsoleColor BC, ConsoleColor FC)
        {
            ColorWriteLine(BC, FC);
            return Console.ReadLine();
        }

        public static string ColorRead(object mess, ConsoleColor BC, ConsoleColor FC)
        {
            ColorWrite(mess, BC, FC);
            return Console.ReadLine();
        }

        public static void ColorWriteLine(object mess, ConsoleColor BC, ConsoleColor FC)
        {
            ConsoleColor cBC = Console.BackgroundColor;
            ConsoleColor cFC = Console.ForegroundColor;
            Console.BackgroundColor = BC;
            Console.ForegroundColor = FC;
            Console.WriteLine(mess);
            Console.ForegroundColor = cFC;
            Console.BackgroundColor = cBC;
        }
        public static void ColorWriteLine(ConsoleColor BC, ConsoleColor FC)
        {
            ConsoleColor cBC = Console.BackgroundColor;
            ConsoleColor cFC = Console.ForegroundColor;
            Console.BackgroundColor = BC;
            Console.ForegroundColor = FC;
            Console.WriteLine();
            Console.ForegroundColor = cFC;
            Console.BackgroundColor = cBC;
        }

        public static void ColorWrite(object mess, ConsoleColor BC, ConsoleColor FC)
        {
            ConsoleColor cBC = Console.BackgroundColor;
            ConsoleColor cFC = Console.ForegroundColor;
            Console.BackgroundColor = BC;
            Console.ForegroundColor = FC;
            Console.Write(mess);
            Console.ForegroundColor = cFC;
            Console.BackgroundColor = cBC;
        }
    }
}
