using System;
using System.Text.RegularExpressions;

namespace tpe
{
    static class CommandLineArgumentsValidator
    {
        static string regexFlag = @"-[d]";

        public static bool IsOpenTaskWithDataFlagAndRealTime(string[] args)
        {
            bool result = args.Length == 6
                 && Regex.IsMatch(args[1], regexFlag)
                 && uint.TryParse(args[2], out uint idTask)
                 && DateTime.TryParse(args[3], out DateTime startDateWork)
                 && TimeParser.TryParse(args[4], out TimeParser plannedWorkTime)
                 && TimeParser.TryParse(args[5], out TimeParser realWorkTime);
            return result;
        }

        public static bool IsOpenTaskWithDataFlagAndNoRealTime(string[] args)
        {
            bool result = args.Length == 5
                && Regex.IsMatch(args[1], regexFlag)
                && uint.TryParse(args[2], out uint idTask)
                && DateTime.TryParse(args[3], out DateTime startDateWork)
                && TimeParser.TryParse(args[4], out TimeParser plannedWorkTime);
            return true;
        }

        public static bool IsOpenTaskWithNoDataFlagAndRealTime(string[] args)
        {
            bool result = args.Length == 4
                && uint.TryParse(args[1], out uint idTask)
                && TimeParser.TryParse(args[2], out TimeParser plannedWorkTime)
                && TimeParser.TryParse(args[3], out TimeParser realWorkTime);
            return true;
        }

        public static bool IsOpenTaskWithNoDataFlagAndNoRealTime(string[] args)
        {
            bool result = args.Length == 3
                && uint.TryParse(args[1], out uint idTask)
                && TimeParser.TryParse(args[2], out TimeParser plannedWorkTime);
            return true;
        }

        public static bool IsOpenTaskWithNoDataFlagAndNoRealTime(string[] args)
        {
            bool result = args.Length == 3
                && uint.TryParse(args[1], out uint idTask)
                && TimeParser.TryParse(args[2], out TimeParser plannedWorkTime);
            return true;
        }
    }
}
