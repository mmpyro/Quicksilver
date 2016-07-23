using System;

namespace Quicksilver.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string message)
        {
            WriteLine(message, ConsoleColor.White);
        }

        public void Error(string message)
        {
            WriteLine(message, ConsoleColor.DarkRed);
        }

        public void Info(string message)
        {
            WriteLine(message, ConsoleColor.DarkGray);
        }

        public void Warn(string message)
        {
            WriteLine(message, ConsoleColor.DarkYellow);
        }

        private void WriteLine(string message,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
