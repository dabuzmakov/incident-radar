using FluentAssertions;
using IncidentRadar.App.Analysis;
using IncidentRadar.App.Parsing;
using System.Net;
using System.Runtime.InteropServices;

namespace IncidentRadar.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: подсчёт статистики.</summary>
public class StatisticsTest
{
    private static readonly string _samplesDirectory
        = Path.Combine(AppContext.BaseDirectory, "samples/logs");

    [Fact(DisplayName = "Распределение кодов: 200,200,404,500,500,500 -> 200:2, 404:1, 500:3")]
    public void ResponseCodesAreCounted()
    {
        var logs = new string[]
        {
            "91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 200 0 \"-\" \"-\"",
            "91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 200 0 \"-\" \"-\"",
            "91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 404 0 \"-\" \"-\"",
            "91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 500 0 \"-\" \"-\"",
            "91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 500 0 \"-\" \"-\"",
            "91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 500 0 \"-\" \"-\"",
        };

        var collector = new StatisticsCollector();

        foreach (var log in logs)
            if (LogParser.TryParse(log, out var logEntry))
                collector.Add(logEntry);

        var statistics = collector.GetStatistics();

        statistics.StatusDistribution.Count.Should().Be(3);
        statistics.StatusDistribution[(HttpStatusCode)200].Should().Be(2);
        statistics.StatusDistribution[(HttpStatusCode)404].Should().Be(1);
        statistics.StatusDistribution[(HttpStatusCode)500].Should().Be(3);
    }

    [Fact(DisplayName = "Top-10 ресурсов отсортирован по убыванию количества")]
    public async Task TopResourcesAreSortedDescending()
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, "access-2025-03-01.log"),
            OutputFormat: "json"
        );

        var result = await LogAnalyzer.AnalyzeAsync(options);

        result.Statistics.TopResources
            .Should().BeInDescendingOrder(pair => pair.Value);
    }

    [Fact(DisplayName = "Размеры 100..500: average 300, max 500, p95 500")]
    public void ResponseSizeStatistics()
    {
        var collector = new StatisticsCollector();

        foreach (var size in new string[] { "100", "200", "300", "400", "500"})
        {
            var log = $"91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 200 {size} \"-\" \"-\"";

            if (LogParser.TryParse(log, out var logEntry))
                collector.Add(logEntry);
        }

        var statistics = collector.GetStatistics();

        statistics.AverageResponseSize.Should().Be(300);
        statistics.MaxResponseSize.Should().Be(500);
        statistics.P95responseSize.Should().Be(500);
    }

    [Fact(DisplayName = "Выбранный метод расчёта p95 зафиксирован и воспроизводим")]
    public async Task P95MethodIsDocumentedAndStable()
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, "access-2025-03-01.log"),
            OutputFormat: "json"
        );

        var firstResult = await LogAnalyzer.AnalyzeAsync(options);
        var secondResult = await LogAnalyzer.AnalyzeAsync(options);

        firstResult.Statistics.P95responseSize
            .Should().Be(secondResult.Statistics.P95responseSize);
    }
}