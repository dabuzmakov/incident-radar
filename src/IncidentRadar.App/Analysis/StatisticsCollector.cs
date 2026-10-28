using IncidentRadar.App.Parsing;
using System.Net;

namespace IncidentRadar.App.Analysis;

public class StatisticsCollector
{
    private static readonly int _topResourcesCount = 10;
    private readonly Dictionary<HttpStatusCode, int> _statusDistribution = new();
    private readonly Dictionary<string, int> _resourcesDistribution = new();
    private readonly Dictionary<long, int> _responseSizeDistribution = new();
    private int _requestsCount;
    private long _totalResponseSize;
    private long _maxResponseSize;

    public void Add(LogEntry entry)
    {
        var (responseSize, status, resource) 
            = (entry.ResponseSize, entry.HttpStatus, entry.Request.Resource);

        _requestsCount++;
        _maxResponseSize = Math.Max(_maxResponseSize, responseSize);
        _totalResponseSize += responseSize;

        _statusDistribution[status] 
            = _statusDistribution.GetValueOrDefault(status) + 1;

        _resourcesDistribution[resource]
            = _resourcesDistribution.GetValueOrDefault(resource) + 1;

        _responseSizeDistribution[responseSize]
            = _responseSizeDistribution.GetValueOrDefault(responseSize) + 1;
    }

    public AnalysisStatistics GetStatistics()
        => new AnalysisStatistics
        (
            RequestsCount: _requestsCount,
            MaxResponseSize: _maxResponseSize,
            StatusDistribution: _statusDistribution,
            TopResources: _resourcesDistribution
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key, StringComparer.Ordinal)
                .Take(_topResourcesCount)
                .ToList(),
            AverageResponseSize: _requestsCount != 0 
                ? _totalResponseSize / (double)_requestsCount 
                : 0,
            P95responseSize: CalculateP95()
        );

    private long CalculateP95()
    {
        if (_requestsCount == 0) return 0;

        var rank = Math.Ceiling(_requestsCount * 0.95);
        var accumulator = 0;

        foreach (var (size, count) in _responseSizeDistribution.OrderBy(x => x.Key))
        {
            accumulator += count;

            if (accumulator >= rank)
                return size;
        }

        throw new InvalidOperationException("Не удалось вычислить P95.");
    }
}
