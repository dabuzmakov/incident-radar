using IncidentRadar.App.Analysis;
using IncidentRadar.App.Analysis.Statistics;
using IncidentRadar.App.CLI;
using IncidentRadar.App.Logging;
using IncidentRadar.App.Parsing;
using IncidentRadar.App.Sources.Opening;
using IncidentRadar.App.Sources.Reading;
using IncidentRadar.App.Sources.Resolving;
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
    public ISourceResolver SourceResolver { get; }
    public ILogParser LogParser { get; }
    public ISourceReader Reader { get; }
    public CommandParser CommandParser { get; }
    public LogAnalyzer Analyzer { get; }

    public AppComposer(
        ILogger? logger = null,
        ILogParser? parser = null,
        ISourceResolver? resolver = null,
        ISourceReader reader = null,
        Func<IStatisticsCollector>? collectorFactory = null)
    {
        CommandParser = new CommandParser(
            new OptionsValidator(_supportedFormats));

        var openerFactory = new SourceOpenerFactory(
            new FileSourceOpener(),
            new UrlSourceOpener());

        Logger = logger ?? new ConsoleLogger();
        SourceResolver = resolver ?? new SourceResolver();
        LogParser = parser ?? new LogParser();
        Reader = reader ?? new SourceReader(openerFactory);

        Analyzer = new LogAnalyzer(
            parser: LogParser,
            reader: Reader,
            collectorFactory: () => new StatisticsCollector(),
            resolver: SourceResolver,
            logger: Logger);
    }
}
