class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the path:");
        var path = Console.ReadLine();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Rename Files");
            Console.WriteLine("2. Undo Rename");
            Console.WriteLine("3. Flush Logs");
            Console.WriteLine("4. Insert");
            Console.WriteLine("5. Format Date");
            Console.WriteLine("6. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RenameService.RenameFiles(path);
                    break;
                case "2":
                    RenameService.UndoRename(path);
                    break;
                case "3":
                    LogsService.FlushLogs();
                    break;
                case "4":
                    RenameService.Insert(path);
                    break;
                case "5":
                    DateService.FormatDate(path);
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }
}
