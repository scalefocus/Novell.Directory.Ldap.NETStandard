using System;

namespace Novell.Directory.Ldap.Samples
{
    internal static class Printer
    {
        public static void Error(string error)
        {
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Reset();
            Console.WriteLine(error);
        }

        public static void PrintAttribute(string name, string value)
        {
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{name}: ");
            Reset();
            Console.WriteLine(value);
        }

        public static void Reset()
        {
            Console.ResetColor();
        }
    }
}
