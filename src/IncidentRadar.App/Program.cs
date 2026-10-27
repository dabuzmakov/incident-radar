using IncidentRadar.App.Analysis;
using IncidentRadar.App.CLI;
using IncidentRadar.App.Exceptions;

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

        try
        {
            var analyzer = new LogAnalyzer(parseResult.Options!);
            var result = await analyzer.AnalyzeAsync();

            return (int)ExitCode.Success;
        }
        catch (SourceException exeption)
        {
            Console.Error.WriteLine($"Ошибка источника: {exeption.Message}");
            return (int)ExitCode.InvalidInput;
        }
        catch (Exception exeption)
        {
            Console.Error.WriteLine($"Внутренняя ошибка: {exeption.Message}");
            return (int)ExitCode.InternalError;
        }
    }
}