using System;

namespace BloodShadow.Core.Logger
{
    public abstract class Logger
    {
        public abstract string ReadLineError(object? mess);
        public abstract string ReadLineWarning(object? mess);
        public abstract string ReadLineMessage(object? mess);

        public abstract string ReadLineError();
        public abstract string ReadLineWarning();
        public abstract string ReadLineMessage();

        public abstract string ReadError(object? mess);
        public abstract string ReadWarning(object? mess);
        public abstract string ReadMessage(object? mess);

        public abstract void WriteLineError(object? mess);
        public abstract void WriteLineWarning(object? mess);
        public abstract void WriteLineMessage(object? mess);

        public abstract void WriteLineError();
        public abstract void WriteLineWarning();
        public abstract void WriteLineMessage();

        public abstract void WriteError(object? mess);
        public abstract void WriteWarning(object? mess);
        public abstract void WriteMessage(object? mess);

        public abstract string ColorReadLine(object? mess, ConsoleColor BC, ConsoleColor FC);
        public abstract string ColorReadLine(ConsoleColor BC, ConsoleColor FC);

        public abstract string ColorRead(object? mess, ConsoleColor BC, ConsoleColor FC);

        public abstract void ColorWriteLine(object? mess, ConsoleColor BC, ConsoleColor FC);
        public abstract void ColorWriteLine(ConsoleColor BC, ConsoleColor FC);

        public abstract void ColorWrite(object? mess, ConsoleColor BC, ConsoleColor FC);
    }
}
