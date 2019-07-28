namespace tpe
{
    static class CommandLineArgumentsValidator
    {
        public static bool IsOpenCommand(string[] args)
        {
            return (args.Length >= 3 && args.Length <= 6) && args[0] == "open";
        }

        public static bool IsCloseCommand(string[] args)
        {
            return (args.Length == 2 || args.Length == 3) && args[0] == "close";
        }

        public static bool IsDeleteCommand(string[] args)
        {
            return args.Length == 2 && args[0] == "del";
        }

        public static bool IsShowCommand(string[] args)
        {
            return args.Length == 1 && args[0] == "show";
        }

        public static bool IsHelpCommand(string[] args)
        {
            return args.Length == 2 && args[0] == "help";
        }

        public static bool IsListCommand(string[] args)
        {
            return args.Length == 1 && args[0] == "list";
        }
    }
}
