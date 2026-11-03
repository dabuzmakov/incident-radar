using System.Net;

namespace IncidentRadar.App.Analysis;

public record AnalysisStatistics(
    int RequestsCount,
    IReadOnlyDictionary<HttpStatusCode, int> StatusDistribution,
    IReadOnlyList<KeyValuePair<string, int>> TopResources,
    double AverageResponseSize,
    long MaxResponseSize,
    long P95responseSize);
