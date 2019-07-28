using System;
using System.IO;

namespace tpe
{
    class Program
    {
        static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".records.csv");

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (CommandLineArgumentsValidator.IsOpenCommand(args))
                    CommandLineInterface.Open(path, args);
                else if (CommandLineArgumentsValidator.IsCloseCommand(args))
                    CommandLineInterface.Close(path, args);
                else if (CommandLineArgumentsValidator.IsDeleteCommand(args))
                    CommandLineInterface.Delete(path, args);
                else if (CommandLineArgumentsValidator.IsShowCommand(args))
                    CommandLineInterface.Show(path);
                else if (CommandLineArgumentsValidator.IsHelpCommand(args))
                    CommandLineInterface.Help(args);
                else if (CommandLineArgumentsValidator.IsListCommand(args))
                    CommandLineInterface.ListCommands();
                else
                {
                    Console.WriteLine("Invalid arguments".PadLeft(20));
                    CommandLineInterface.Help(args);
                }
            }
            else
                CommandLineInterface.ListCommands();
        }
    }
}