using IncidentRadar.App.Analysis.Statistics;
using IncidentRadar.App.Logging;
using IncidentRadar.App.Parsing;
using IncidentRadar.App.Sources.Reading;
using IncidentRadar.App.Sources.Resolving;

namespace IncidentRadar.App.Analysis;

public class LogAnalyzer
{
    private readonly ILogParser _parser;
    private readonly ILogger _logger;
    private readonly ISourceResolver _resolver;
    private readonly ISourceReader _reader;
    private readonly Func<IStatisticsCollector> _collectorFactory;

    public LogAnalyzer(
       ILogParser parser,
       ISourceReader reader,
       Func<IStatisticsCollector> collectorFactory,
       ISourceResolver resolver,
       ILogger logger)
    {
        _parser = parser;
        _reader = reader;
        _collectorFactory = collectorFactory;
        _resolver = resolver;
        _logger = logger;
    }

    public async Task<AnalysisResult> AnalyzeAsync(AnalysisOptions options)
    {
        var sources = _resolver.GetSources(options.Path);
        var collector = _collectorFactory();

        var (totalRead, malformedCount) = (0, 0);

        await foreach (var line in _reader.ReadAsync(sources))
        {
            totalRead++;

            if (!_parser.TryParse(line, out var logEntry))
            {
                malformedCount++;
                _logger.LogWarning($"пропущена некорректная строка {totalRead}");
                continue;
            }

            if (!logEntry.InBounds(options.From, options.To))
            {
                continue;
            }

            collector.Add(logEntry);
        }

        return new AnalysisResult
        (
            Sources: sources,
            TotalRead: totalRead,
            MalformedCount: malformedCount,
            Statistics: collector.GetStatistics()
        );
    }


}
