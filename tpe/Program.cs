using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace tpe
{
    class Program
    {
        static string regexFlag = @"-[d]";
        static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".records.csv");
        static void Main(string[] args)
        {
            Data data = new Data(path);
            data.FromFile();
            if (args.Length != 0)
            {
                if (args[0] == "open")
                    OpenTask(data, args);
                else if (args[0] == "close")
                    CloseTask(data, args);
                else if (args[0] == "del")
                    DeleteTask(data, args);
                else if (args[0] == "show")
                    ShowTask(data, args);
                else if (args[0] == "help")
                    Help(args);
                else if (args[0] == "list")
                    ListCommands();
                else
                    Help(args);
            }
            else
                ListCommands();
        }

        static void OpenTask(Data data, params string[] args)
        {
            if (args.Length == 6
                && Regex.IsMatch(args[1], regexFlag)
                && uint.TryParse(args[2], out uint id)
                && DateTime.TryParse(args[3], out DateTime date)
                && TimeParser.TryParse(args[4], out TimeParser planned)
                && TimeParser.TryParse(args[5], out TimeParser real))
            {
                if (data.Open(new Record(id, date, planned, real)))
                    data.ToFile();
                else
                    Console.WriteLine("Id exists".PadLeft(12));
            }   
            else if (args.Length == 5
                && Regex.IsMatch(args[1], regexFlag)
                && uint.TryParse(args[2], out id)
                && DateTime.TryParse(args[3], out date)
                && TimeParser.TryParse(args[4], out planned))
            {
                if (data.Open(new Record(id, date, planned)))
                    data.ToFile();
                else
                    Console.WriteLine("Id exists".PadLeft(12));
            }
            else if (args.Length == 4
                && uint.TryParse(args[1], out id)
                && TimeParser.TryParse(args[2], out planned)
                && TimeParser.TryParse(args[3], out real))
            {
                if (data.Open(new Record(id, planned, real)))
                    data.ToFile();
                else
                    Console.WriteLine("Id exists".PadLeft(12));
            }
            else if (args.Length == 3
                && uint.TryParse(args[1], out id)
                && TimeParser.TryParse(args[2], out planned))
            {
                if (data.Open(new Record(id, planned)))
                    data.ToFile();
                else
                    Console.WriteLine("Id exists".PadLeft(12));
            }
            else
                Console.WriteLine("Invalid arguments".PadLeft(20));
        }

        static void CloseTask(Data data, params string[] args)
        {
            if (File.Exists(path))
            {
                if (args.Length == 3 
                    && uint.TryParse(args[1], out uint id)
                    && TimeParser.TryParse(args[2], out TimeParser real))
                {
                    if (data.Close(id, real))
                    {
                        Console.WriteLine("Task close".PadLeft(13));
                        data.ToFile();
                    }
                    else
                        Console.WriteLine("ID not find".PadLeft(14));
                }
                else if (args.Length == 2
                    && uint.TryParse(args[1], out id))
                {
                    if (data.Close(id))
                    {
                        Console.WriteLine("Task close".PadLeft(13));
                        data.ToFile();
                    }
                    else
                        Console.WriteLine("ID not find".PadLeft(14));
                }
                else
                    Console.WriteLine("Invalid arguments".PadLeft(20));
            }
            else
                Console.WriteLine("File not find".PadLeft(16));
        }

        static void DeleteTask(Data data, params string[] args)
        {
            if (File.Exists(path))
            {
                if (args.Length == 2 && uint.TryParse(args[1], out uint id))
                {
                    if (data.Remove(data.Find(id)))
                    {
                        data.ToFile();
                        Console.WriteLine("Task remove".PadLeft(14));
                    }
                    else
                        Console.WriteLine("ID not find".PadLeft(14));
                }
                else
                    Console.WriteLine("Invalid arguments".PadLeft(20));
            }
            else
                Console.WriteLine("File not find".PadLeft(16));
        }

        static void ShowTask(Data data, params string[] args)
        {
            if (File.Exists(path))
            {
                if (args.Length == 1)
                    data.Show();
                else
                    Console.WriteLine("Invalid arguments".PadLeft(20));
            }
            else
                Console.WriteLine("File not find".PadLeft(16));
        }

        static void ListCommands()
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

        static void Help(params string[] args)
        {
            if (args.Length == 2 && args[1] == "open")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("open <id> <planned time> [real time]".PadLeft(41));
                Console.WriteLine("open [-d] <id> <date> <planned time> [real time]".PadLeft(53));
            }
            else if (args.Length == 2 && args[1] == "close")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("close <id> <real time>".PadLeft(27));
                Console.WriteLine("close <id>".PadLeft(15));
            }
            else if (args.Length == 2 && args[1] == "del")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("del <id>".PadLeft(13));
            }
            else if (args.Length == 2 && args[1] == "show")
            {
                Console.WriteLine("Pattern:".PadLeft(11));
                Console.WriteLine("show".PadLeft(9));
            }        
            else if (args.Length == 2 && args[1] == "list")
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