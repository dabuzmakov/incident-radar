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
        var resolver = new SourceResolver(_options.Path);
        var reader = new SourceReader(resolver);

        var totalRead = 0;

        await foreach (var line in reader.ReadAsync())
        {
            totalRead++;
        }

        return new AnalysisStatistics
        (
            Sources: resolver.Sources,
            TotalRead: totalRead
        );
    }
}
