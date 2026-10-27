using IncidentRadar.App.Sources;

namespace IncidentRadar.App.Analysis;

public record AnalysisStatistics(IReadOnlyList<LogSource> Sources, int TotalRead);
