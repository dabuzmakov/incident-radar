using IncidentRadar.App.Parsing;
using IncidentRadar.App.Sources;

namespace IncidentRadar.App.Analysis;

public class LogAnalyzer
{
    private readonly AnalysisOptions _options;

    public LogAnalyzer(AnalysisOptions options)
    {
        _options = options;
    }

    public async Task<AnalysisStatistics> AnalyzeAsync()
    {
        var sources = SourceResolver.GetSources(_options.Path);
        var reader = new SourceReader(sources);

        var (totalRead, malformedCount) = (0, 0);

        await foreach (var line in reader.ReadAsync())
        {
            totalRead++;

            if (!LogParser.TryParse(line, out var log))
            {
                malformedCount++;
                Console.Error.WriteLine($"Пропущена некорректная строка, номер {totalRead - 1}");
                continue;
            }


        }

        return new AnalysisStatistics
        (
            Sources: sources,
            TotalRead: totalRead
        );
    }
}
