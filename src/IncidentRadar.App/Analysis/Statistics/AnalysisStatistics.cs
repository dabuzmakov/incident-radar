using System.Net;

namespace IncidentRadar.App.Analysis.Statistics;

public record AnalysisStatistics(
    int RequestsCount,
    IReadOnlyDictionary<HttpStatusCode, int> StatusDistribution,
    IReadOnlyList<KeyValuePair<string, int>> TopResources,
    HashSet<string> Protocols,
    double AverageResponseSize,
    long MaxResponseSize,
    long P95responseSize);
