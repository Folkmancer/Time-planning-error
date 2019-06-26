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
                if (args[0] == "add")
                    AddTask(data, args);
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

        static void AddTask(Data data, params string[] args)
        {
            if (args.Length == 6
                && Regex.IsMatch(args[1], regexFlag)
                && uint.TryParse(args[2], out uint id)
                && DateTime.TryParse(args[3], out DateTime date)
                && Time.TryParse(args[4], out Time planned)
                && Time.TryParse(args[5], out Time real))
            {
                if (data.Add(new Record(id, date, planned, real)))
                    data.ToFile();
                else
                    Console.WriteLine("   Id exists");
            }   
            else if (args.Length == 5
                && Regex.IsMatch(args[1], regexFlag)
                && uint.TryParse(args[2], out id)
                && DateTime.TryParse(args[3], out date)
                && Time.TryParse(args[4], out planned))
            {
                if (data.Add(new Record(id, date, planned)))
                    data.ToFile();
                else
                    Console.WriteLine("   Id exists");
            }
            else if (args.Length == 4
                && uint.TryParse(args[1], out id)
                && Time.TryParse(args[2], out planned)
                && Time.TryParse(args[3], out real))
            {
                if (data.Add(new Record(id, planned, real)))
                    data.ToFile();
                else
                    Console.WriteLine("   Id exists");
            }
            else if (args.Length == 3
                && uint.TryParse(args[1], out id)
                && Time.TryParse(args[2], out planned))
            {
                if (data.Add(new Record(id, planned)))
                    data.ToFile();
                else
                    Console.WriteLine("   Id exists");
            }
            else
                Console.WriteLine("   Invalid arguments");
        }

        static void CloseTask(Data data, params string[] args)
        {
            if (File.Exists(path))
            {
                if (args.Length == 3 
                    && uint.TryParse(args[1], out uint id)
                    && Time.TryParse(args[2], out Time real))
                {
                    if (data.Close(id, real))
                    {
                        Console.WriteLine("   Task close");
                        data.ToFile();
                    }
                    else
                        Console.WriteLine("   ID not find");
                }
                else
                    Console.WriteLine("   Invalid arguments");
            }
            else
                Console.WriteLine("   File not find");
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
                        Console.WriteLine("   Task remove");
                    }
                    else
                        Console.WriteLine("   ID not find");
                }
                else
                    Console.WriteLine("   Invalid arguments");
            }
            else
                Console.WriteLine("   File not find");
        }

        static void ShowTask(Data data, params string[] args)
        {
            if (File.Exists(path))
            {
                if (args.Length == 1)
                    data.Show();
                else
                    Console.WriteLine("   Invalid arguments");
            }
            else
                Console.WriteLine("   File not find");
        }

        static void ListCommands()
        {
            StringBuilder commands = new StringBuilder();
            Console.WriteLine("   You can use next comands:");
            commands.AppendLine("     add - adding new task");
            commands.AppendLine("     close - adding real work time for task");
            commands.AppendLine("     del - delete task");
            commands.AppendLine("     show - show all task");
            commands.AppendLine("     list - show all available commands");
            commands.AppendLine("     help - help for any commands");
            Console.WriteLine(commands);
        }

        static void Help(params string[] args)
        {
            if (args.Length == 2 && args[1] == "add")
            {
                Console.WriteLine("   Pattern:");
                Console.WriteLine("     add <id> <planned time> [real time]");
                Console.WriteLine("     add [-d] <id> <date> <planned time> [real time]");
            }
            else if (args.Length == 2 && args[1] == "close")
            {
                Console.WriteLine("   Pattern:");
                Console.WriteLine("     close <id> <real time>");
            }
            else if (args.Length == 2 && args[1] == "del")
            {
                Console.WriteLine("   Pattern:");
                Console.WriteLine("     del <id>");
            }
            else if (args.Length == 2 && args[1] == "show")
            {
                Console.WriteLine("   Pattern:");
                Console.WriteLine("     show");
            }        
            else if (args.Length == 2 && args[1] == "list")
            {
                Console.WriteLine("   Pattern:");
                Console.WriteLine("     list");
            }
            else
            {
                Console.WriteLine("   For help use next pattern:");
                Console.WriteLine("     help <command>");
            }
        }
    }
}