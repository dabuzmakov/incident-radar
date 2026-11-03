using IncidentRadar.App.Analysis;
using IncidentRadar.App.CLI;
using IncidentRadar.App.Exceptions;
using System.Text;

public class Program
{
    private static readonly Dictionary<string, string> _supportedFormats = new()
    {
        ["json"] = ".json",
        ["markdown"] = ".md"
    };

    public static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var optionsValidator = new OptionsValidator(_supportedFormats);
        var commandParser = new CommandParser(optionsValidator);
        var parseResult = commandParser.Parse(args);

        if (!parseResult.IsSuccess)
        {
            foreach (var error in parseResult.Errors)
            {
                Console.Error.WriteLine(error);
            }

            return (int)ExitCode.InvalidInput;
        }

        try
        {
            var result = await LogAnalyzer.AnalyzeAsync(parseResult.Options!);

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