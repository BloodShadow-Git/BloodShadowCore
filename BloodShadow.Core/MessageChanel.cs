using System.Diagnostics.CodeAnalysis;

namespace BloodShadow.Core
{
    public struct MessageChanel(string name, int level)
    {
        public string Name { get; private set; } = name;
        public int Level { get; private set; } = level;
        public readonly string LoggerString => $"[{Level}-{Name}]";

        public static readonly MessageChanel INFO = new("INFO", 0);
        public static readonly MessageChanel DEBUG = new("DEBUG", 1);
        public static readonly MessageChanel WARN = new("WARN", 2);
        public static readonly MessageChanel ERROR = new("ERROR", 3);
        public static readonly MessageChanel FATAL = new("FATAL", 4);

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is MessageChanel chanel && chanel.LoggerString.Equals(LoggerString);
        public override readonly int GetHashCode() => LoggerString.GetHashCode();

        public static bool operator ==(MessageChanel chanel1, MessageChanel chanel2) => chanel1.LoggerString == chanel2.LoggerString;
        public static bool operator !=(MessageChanel chanel1, MessageChanel chanel2) => chanel1.LoggerString != chanel2.LoggerString;
    }
}
