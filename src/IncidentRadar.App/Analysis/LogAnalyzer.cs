using IncidentRadar.App.Parsing;
using IncidentRadar.App.Sources;

namespace IncidentRadar.App.Analysis;

public static class LogAnalyzer
{
    public static async Task<AnalysisResult> AnalyzeAsync(AnalysisOptions options)
    {
        var sources = SourceResolver.GetSources(options.Path);
        var reader = new SourceReader(sources);
        var collector = new StatisticsCollector();

        var (totalRead, malformedCount) = (0, 0);

        await foreach (var line in reader.ReadAsync())
        {
            totalRead++;

            if (!LogParser.TryParse(line, out var logEntry))
            {
                malformedCount++;
                Console.Error.WriteLine($"Пропущена некорректная строка, номер {totalRead - 1}");
                continue;
            }

            if (!logEntry.InBounds(options.From, options.To))
                continue;

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
