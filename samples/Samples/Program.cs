using System;

namespace Novell.Directory.Ldap.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Search.Run(args);

            Console.WriteLine("Press a key...");
            Console.ReadKey();
        }
    }
}