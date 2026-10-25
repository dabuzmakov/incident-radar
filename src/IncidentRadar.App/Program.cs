using IncidentRadar.App.Analysis;
using IncidentRadar.App.CLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var parseResult = CommandParser.Parse(args);

        if (!parseResult.IsSuccess)
        {
            foreach (var error in parseResult.Errors)
                Console.Error.WriteLine(error);

            return (int)ExitCode.InvalidInput;
        }

        var analyzer = new LogAnalyzer(parseResult.Options!);

        return (int)ExitCode.Success;
    }
}