using static System.Console;


namespace DirectoriesComparer;

internal class Program
{
    private static StreamWriter? _logger;
    
    static void Main(string[] args)
    {
        switch (args.Length)
        {
            case < 2 or >3:
                ShowHelp();
                return;
            case 3:
                _logger = new StreamWriter(args[2])
                {
                    AutoFlush = true
                };
                break;
        }

        var dir1 = new DirInfo(args[0]);
        var dir2 = new DirInfo(args[1]);

        var comparer = new DirComparer(dir1, dir2);
        comparer.OnProgressChanged += PrintProgress;
        var result = comparer.Compare()? "equal" : "not equal";

        LogWriteLine($"Folders are {result}\r\n");
    }

    private static void ShowHelp()
    {
        var appName = AppDomain.CurrentDomain.FriendlyName + ".exe";
        WriteLine("Directories comparer ver. 0.9b\r\n");
        WriteLine($"USAGE: {appName} directory1 directory2 [logfile]");

    }

    private static void PrintProgress(object? sender, CompareProgressEventArg arg)
    {
        if (string.IsNullOrWhiteSpace(arg.StatusMessage)) 
            return;

        LogWriteLine(arg.StatusMessage);
    }

    private static void LogWriteLine(string message)
    {
        WriteLine(message);
        _logger?.WriteLine(message);
    }
}