namespace BloodFramework
{
    public static class Logger
    {
        public static string? ReadLineError(object? mess) { return ColorReadLine(mess, ConsoleColor.Red, ConsoleColor.Black); }
        public static string? ReadLineWarning(object? mess) { return ColorReadLine(mess, ConsoleColor.Yellow, ConsoleColor.Black); }
        public static string? ReadLineMessage(object? mess) { return ColorReadLine(mess, ConsoleColor.White, ConsoleColor.Black); }
        public static string? ReadError(object? mess) { return ColorRead(mess, ConsoleColor.Red, ConsoleColor.Black); }
        public static string? ReadWarning(object? mess) { return ColorRead(mess, ConsoleColor.Yellow, ConsoleColor.Black); }
        public static string? ReadMessage(object? mess) { return ColorRead(mess, ConsoleColor.White, ConsoleColor.Black); }
        public static void WriteLineError(object? mess) { ColorWriteLine(mess, ConsoleColor.Red, ConsoleColor.Black); }
        public static void WriteLineWarning(object? mess) { ColorWriteLine(mess, ConsoleColor.Yellow, ConsoleColor.Black); }
        public static void WriteLineMessage(object? mess) { ColorWriteLine(mess, ConsoleColor.White, ConsoleColor.Black); }
        public static void WriteError(object? mess) { ColorWrite(mess, ConsoleColor.Red, ConsoleColor.Black); }
        public static void WriteWarning(object? mess) { ColorWrite(mess, ConsoleColor.Yellow, ConsoleColor.Black); }
        public static void WriteMessage(object? mess) { ColorWrite(mess, ConsoleColor.White, ConsoleColor.Black); }

        public static string? ColorReadLine(object? mess, ConsoleColor BC, ConsoleColor FC)
        {
            ColorWriteLine(mess, BC, FC);
            return Console.ReadLine();
        }

        public static string? ColorRead(object? mess, ConsoleColor BC, ConsoleColor FC)
        {
            ColorWrite(mess, BC, FC);
            return Console.ReadLine();
        }

        public static void ColorWriteLine(object? mess, ConsoleColor BC, ConsoleColor FC)
        {
            ConsoleColor cBC = Console.BackgroundColor;
            ConsoleColor cFC = Console.ForegroundColor;
            Console.BackgroundColor = BC;
            Console.ForegroundColor = FC;
            Console.WriteLine(mess);
            Console.ForegroundColor = cFC;
            Console.BackgroundColor = cBC;
        }

        public static void ColorWrite(object? mess, ConsoleColor BC, ConsoleColor FC)
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
