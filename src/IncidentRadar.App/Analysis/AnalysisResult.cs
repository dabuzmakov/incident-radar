using IncidentRadar.App.Sources;

namespace IncidentRadar.App.Analysis;

public record AnalysisResult(
    AnalysisStatistics Statistics,
    IReadOnlyList<LogSource> Sources,
    int TotalRead,
    int MalformedCount);
