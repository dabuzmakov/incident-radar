using IncidentRadar.App.Sources;

namespace IncidentRadar.App.Analysis;

public class LogAnalyzer
{
    private readonly AnalysisOptions _options;

    public LogAnalyzer(AnalysisOptions options)
    {
        _options = options;
    }

    public async Task AnalyzeAsync()
    {
        var resolver = new SourceResolver(_options.Path);
        var reader = new SourceReader(resolver.GetSources());
    }
}
