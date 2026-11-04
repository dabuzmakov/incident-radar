using IncidentRadar.App.Analysis;
using IncidentRadar.App.Analysis.Statistics;
using IncidentRadar.App.CLI;
using IncidentRadar.App.Logging;
using IncidentRadar.App.Parsing;
using IncidentRadar.App.Sources.Opening;
using IncidentRadar.App.Sources.Reading;
using IncidentRadar.App.Sources.Resolving;
using System.Text;

namespace IncidentRadar.App;

public class AppComposer
{
    private static readonly IReadOnlyDictionary<string, string> _supportedFormats =
        new Dictionary<string, string>
        {
            ["json"] = ".json",
            ["markdown"] = ".md"
        };

    public ILogger Logger { get; }

    public CommandParser CommandParser { get; }

    public LogAnalyzer Analyzer { get; }

    public AppComposer(
        ILogger? logger = null,
        ILogParser? parser = null,
        ISourceResolver? resolver = null)
    {
        Logger = logger ?? new ConsoleLogger();

        CommandParser = new CommandParser(
            new OptionsValidator(_supportedFormats));

        var openerFactory = new SourceOpenerFactory(
            new FileSourceOpener(),
            new UrlSourceOpener());

        Analyzer = new LogAnalyzer(
            parser: parser ?? new LogParser(),
            readerFactory: sources => new SourceReader(sources, openerFactory),
            collectorFactory: () => new StatisticsCollector(),
            resolver: resolver ?? new SourceResolver(),
            logger: Logger);
    }
}
