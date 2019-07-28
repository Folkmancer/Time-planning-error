using System;
using System.Text;

namespace tpe
{
    public static class CommandLineInterface
    {
        public static void Open(string path, string[] args)
        {
            TaskManager data = new TaskManager();
            DataLoader.FromFile(path, data);
            if (Record.TryParse(args, out Record record))
            {
                if (data.ExistsTask(record.Id))
                    data.OpenTask(record);
                else
                    Console.WriteLine("Id exists".PadLeft(12));
            }
            else
                Console.WriteLine("Invalid arguments".PadLeft(20));
            DataLoader.ToFile(path, data);
        }

        public static void Close(string path, string[] args)
        {
            TaskManager data = new TaskManager();
            DataLoader.FromFile(path, data);
            if (data.IsEmpty())
            {
                if (args.Length == 3
                    && uint.TryParse(args[1], out uint id)
                    && TimeParser.TryParse(args[2], out TimeSpan real))
                {
                    if (data.CloseTask(id, real))
                        Console.WriteLine("Task close".PadLeft(13));
                    else
                        Console.WriteLine("ID not find".PadLeft(14));
                }
                else if (args.Length == 2
                    && uint.TryParse(args[1], out id))
                {
                    if (data.CloseTask(id))
                        Console.WriteLine("Task close".PadLeft(13));
                    else
                        Console.WriteLine("ID not find".PadLeft(14));
                }
                else
                    Console.WriteLine("Invalid arguments".PadLeft(20));
            }
            else
                Console.WriteLine("File not find".PadLeft(16));
            DataLoader.ToFile(path, data);
        }

        public static void Delete(string path, string[] args)
        {
            TaskManager data = new TaskManager();
            DataLoader.FromFile(path, data);
            if (data.IsEmpty())
            {
                if (uint.TryParse(args[1], out uint id))
                {
                    if (data.RemoveTask(id))
                        Console.WriteLine("Task remove".PadLeft(14));
                    else
                        Console.WriteLine("ID not find".PadLeft(14));
                }
                else
                    Console.WriteLine("Invalid arguments".PadLeft(20));
            }
            else
                Console.WriteLine("File not find".PadLeft(16));
            DataLoader.ToFile(path, data);
        }

        public static void Show(string path)
        {
            TaskManager data = new TaskManager();
            DataLoader.FromFile(path, data);
            if (data.IsEmpty())
                data.ShowTasks();
            else
                Console.WriteLine("File not find".PadLeft(16));
        }

        public static void ListCommands()
        {
            StringBuilder commands = new StringBuilder();
            Console.WriteLine("You can use next comands:".PadLeft(28));
            commands.AppendLine("open - adding new task".PadLeft(27));
            commands.AppendLine("close - adding real work time for task".PadLeft(43));
            commands.AppendLine("del - delete task".PadLeft(22));
            commands.AppendLine("show - show all task".PadLeft(25));
            commands.AppendLine("list - show all available commands".PadLeft(39));
            commands.AppendLine("help - help for any commands".PadLeft(33));
            Console.WriteLine(commands);
        }

        public static void Help(string[] args)
        {
            if (args[1] == "open")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("open <id> <planned time> [real time]".PadLeft(41));
                Console.WriteLine("open [-d] <id> <date> <planned time> [real time]".PadLeft(53));
            }
            else if (args[1] == "close")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("close <id> <real time>".PadLeft(27));
                Console.WriteLine("close <id>".PadLeft(15));
            }
            else if (args[1] == "del")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("del <id>".PadLeft(13));
            }
            else if (args[1] == "show")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("show".PadLeft(9));
            }
            else if (args[1] == "list")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("list".PadLeft(9));
            }
            else
            {
                Console.WriteLine("For help use next pattern:".PadLeft(29));
                Console.WriteLine("help <command>".PadLeft(19));
            }
        }
    }
}
