using IncidentRadar.App.Sources;
using System.Net;

namespace IncidentRadar.App.Analysis;

public record AnalysisStatistics(
    int RequestsCount,
    Dictionary<HttpStatusCode, int> StatusDistribution,
    List<KeyValuePair<string, int>> TopResources,
    double AverageResponseSize,
    long MaxResponseSize,
    long P95responseSize);
