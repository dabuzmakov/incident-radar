using IncidentRadar.App.Analysis;
using System.Text.Json;

namespace IncidentRadar.App.Reports;

public class JsonReportBuilder : IReportBuilder
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public string CreateReport(AnalysisResult analysisResult)
    {
        var statistics = analysisResult.Statistics;

        var report = new
        {
            files = analysisResult.Sources.Select(source => source.Path).ToArray(),
            filesCount = analysisResult.Sources.Count,
            processedLinesCount = analysisResult.TotalRead,
            malformedLinesCount = analysisResult.MalformedCount,
            totalRequestsCount = statistics.RequestsCount,
            responseSizeInBytes = new
            {
                average = statistics.AverageResponseSize,
                max = statistics.MaxResponseSize,
                p95 = statistics.P95responseSize
            },
            resources = statistics.TopResources.Select(pair => new
            {
                resource = pair.Key,
                count = pair.Value
            }).ToArray(),
            responseCodes = statistics.StatusDistribution
                .OrderBy(pair => (int)pair.Key)
                .Select(pair => new
                {
                    code = (int)pair.Key,
                    name = pair.Key.ToString(),
                    count = pair.Value
                }).ToArray(),
            protocols = statistics.Protocols.OrderBy(value => value).ToArray()
        };

        return JsonSerializer.Serialize(report, _options);
    }
}
