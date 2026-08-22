using BloodShadow.Core;

namespace BloodShadow.Core.ConsoleInterface
{
    public class DefaultConsoleInterface : ConsoleInterface
    {
        public const ConsoleColor EXP_FORE_COLOR = ConsoleColor.Red;
        public const ConsoleColor EXP_BACK_COLOR = ConsoleColor.Black;
        public const ConsoleColor WARN_FORE_COLOR = ConsoleColor.Yellow;
        public const ConsoleColor WARN_BACK_COLOR = ConsoleColor.Black;
        public const ConsoleColor MSG_FORE_COLOR = ConsoleColor.White;
        public const ConsoleColor MSG_BACK_COLOR = ConsoleColor.Black;

        protected override string? ReadInternal() => Console.ReadLine();
        protected override void WriteInternal(object? mess, MessageChanel chanel)
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
            Console.Write(mess);
            Console.ForegroundColor = cFC;
            Console.BackgroundColor = cBC;
        }
    }
}
