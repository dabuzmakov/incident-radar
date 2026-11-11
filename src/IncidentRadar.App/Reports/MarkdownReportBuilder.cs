using IncidentRadar.App.Analysis;
using IncidentRadar.App.Analysis.Statistics;
using System.Globalization;
using System.Text;

namespace IncidentRadar.App.Reports;

public class MarkdownReportBuilder : IReportBuilder
{
    public string CreateReport(AnalysisResult analysisResult)
    {
        var builder = new StringBuilder();

        AppendSummary(builder, analysisResult);
        AppendFiles(builder, analysisResult);
        AppendResponseSizes(builder, analysisResult.Statistics);
        AppendResources(builder, analysisResult.Statistics);
        AppendResponseCodes(builder, analysisResult.Statistics);
        AppendProtocols(builder, analysisResult.Statistics);

        return builder.ToString();
    }

    private static void AppendSummary(StringBuilder builder, AnalysisResult result)
    {
        builder.AppendLine("# Incident Radar");
        builder.AppendLine();
        builder.AppendLine($"- Files count: {result.Sources.Count}");
        builder.AppendLine($"- Processed lines: {result.TotalRead}");
        builder.AppendLine($"- Malformed lines: {result.MalformedCount}");
        builder.AppendLine($"- Total requests: {result.Statistics.RequestsCount}");
        builder.AppendLine();
    }

    private static void AppendFiles(StringBuilder builder, AnalysisResult result)
    {
        builder.AppendLine("## Files");
        foreach (var source in result.Sources)
        {
            builder.AppendLine($"- {source.Path}");
        }

        builder.AppendLine();
    }

    private static void AppendResponseSizes(StringBuilder builder, AnalysisStatistics statistics)
    {
        builder.AppendLine("## Response size in bytes");
        builder.AppendLine($"- Average: {statistics.AverageResponseSize.ToString("F2", CultureInfo.InvariantCulture)}");
        builder.AppendLine($"- Max: {statistics.MaxResponseSize}");
        builder.AppendLine($"- P95: {statistics.P95responseSize}");
        builder.AppendLine();
    }

    private static void AppendResources(StringBuilder builder, AnalysisStatistics statistics)
    {
        builder.AppendLine("## Resources (top 10)");
        builder.AppendLine("| Resource | Count |");
        builder.AppendLine("| --- | ---: |");
        foreach (var (resource, count) in statistics.TopResources)
        {
            builder.AppendLine($"| {resource} | {count} |");
        }

        builder.AppendLine();
    }

    private static void AppendResponseCodes(StringBuilder builder, AnalysisStatistics statistics)
    {
        builder.AppendLine("## Response codes");
        builder.AppendLine("| Code | Name | Count |");
        builder.AppendLine("| --- | --- | ---: |");
        foreach (var (status, count) in statistics.StatusDistribution.OrderBy(pair => (int)pair.Key))
        {
            builder.AppendLine($"| {(int)status} | {status} | {count} |");
        }

        builder.AppendLine();
    }

    private static void AppendProtocols(StringBuilder builder, AnalysisStatistics statistics)
    {
        builder.AppendLine("## Protocols");
        foreach (var protocol in statistics.Protocols.OrderBy(value => value))
        {
            builder.AppendLine($"- {protocol}");
        }
    }
}
