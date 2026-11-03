using IncidentRadar.App.Analysis;
using IncidentRadar.App.CLI;
using IncidentRadar.App.Exceptions;
using IncidentRadar.App.Logging;
using IncidentRadar.App.Parsing;
using IncidentRadar.App.Sources;
using IncidentRadar.App.Sources.Opening;
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

        var logger = new ConsoleLogger();

        var optionsValidator = new OptionsValidator(_supportedFormats);
        var commandParser = new CommandParser(optionsValidator);

        var openerFactory = new SourceOpenerFactory(
            new FileSourceOpener(),
            new UrlSourceOpener()
        );

        var analyzer = new LogAnalyzer(
            parser: new LogParser(),
            readerFactory: sources => new SourceReader(sources, openerFactory),
            collectorFactory: () => new StatisticsCollector(),
            resolver: new SourceResolver(),
            logger: logger
        );

        var parseResult = commandParser.Parse(args);

        if (!parseResult.IsSuccess)
        {
            foreach (var error in parseResult.Errors)
            {
                logger.LogError(error);
            }

            return (int)ExitCode.InvalidInput;
        }

        try
        {
            var result = await analyzer.AnalyzeAsync(parseResult.Options!);

            return (int)ExitCode.Success;
        }
        catch (SourceException exсeption)
        {
            logger.LogError($"ошибка источника, {exсeption.Message}");
            return (int)ExitCode.InvalidInput;
        }
        catch (Exception exсeption)
        {
            logger.LogError($"внутренняя ошибка, {exсeption.Message}");
            return (int)ExitCode.InternalError;
        }
    }
}