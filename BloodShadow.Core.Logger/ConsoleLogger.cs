using System.Diagnostics;

namespace BloodShadow.Core.Logger
{
    public class ConsoleLogger : Logger
    {
        public const ConsoleColor EXP_FORE_COLOR = ConsoleColor.Red;
        public const ConsoleColor EXP_BACK_COLOR = ConsoleColor.Black;
        public const ConsoleColor WARN_FORE_COLOR = ConsoleColor.Yellow;
        public const ConsoleColor WARN_BACK_COLOR = ConsoleColor.Black;
        public const ConsoleColor MSG_FORE_COLOR = ConsoleColor.White;
        public const ConsoleColor MSG_BACK_COLOR = ConsoleColor.Black;

        protected override void WriteInternal(MessageChanel chanel, LoggerLabel label, string message, StackTrace? stackTrace, Exception? exception)
        {
            ConsoleColor cBC = Console.BackgroundColor;
            ConsoleColor cFC = Console.ForegroundColor;
            if (chanel == MessageChanel.INFO)
            {
                Console.BackgroundColor = MSG_BACK_COLOR;
                Console.ForegroundColor = MSG_FORE_COLOR;
            }
            else if (chanel == MessageChanel.DEBUG)
            {
                Console.BackgroundColor = MSG_BACK_COLOR;
                Console.ForegroundColor = MSG_FORE_COLOR;
            }
            else if (chanel == MessageChanel.WARN)
            {
                Console.BackgroundColor = WARN_BACK_COLOR;
                Console.ForegroundColor = WARN_FORE_COLOR;
            }
            else if (chanel == MessageChanel.ERROR)
            {
                Console.BackgroundColor = EXP_BACK_COLOR;
                Console.ForegroundColor = EXP_FORE_COLOR;
            }
            else if (chanel == MessageChanel.FATAL)
            {
                Console.BackgroundColor = EXP_BACK_COLOR;
                Console.ForegroundColor = EXP_FORE_COLOR;
            }
            else
            {
                Console.BackgroundColor = EXP_BACK_COLOR;
                Console.ForegroundColor = EXP_FORE_COLOR;
            }
            Console.Write($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}-{chanel.LoggerString}[{chanel.Level}]] [{label.Label}] {message}\n{stackTrace}\n{exception}");
            Console.ForegroundColor = cFC;
            Console.BackgroundColor = cBC;
        }
    }
}
