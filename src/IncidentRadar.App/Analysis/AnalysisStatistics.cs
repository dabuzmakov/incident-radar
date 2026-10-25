using IncidentRadar.App.Sources;

namespace IncidentRadar.App.Analysis;

public record AnalysisStatistics(IEnumerable<LogSource> Sources, int TotalRead);
