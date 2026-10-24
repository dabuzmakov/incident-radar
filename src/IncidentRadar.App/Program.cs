using IncidentRadar.App.Analysis;
using IncidentRadar.App.CLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var parseResult = CommandParser.Parse(args);

        if (!parseResult.Successed)
        {
            foreach (var error in parseResult.Errors)
                Console.Error.WriteLine(error);

            return (int)ExitCode.InvalidInput;
        }

        var options = parseResult.ToAnalysisOptions();
        var analyzer = new NginxLogAnalyzer(options);

        return (int)ExitCode.Success;
    }
}