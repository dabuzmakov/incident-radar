using FluentAssertions;
using IncidentRadar.App.Analysis;
using IncidentRadar.App.Parsing;
using System.Net;

namespace IncidentRadar.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: подсчёт статистики.</summary>
public class StatisticsTest
{
    private static readonly string _samplesDirectory
        = Path.Combine(AppContext.BaseDirectory, "samples/logs");

    [Fact(DisplayName = "Распределение кодов: 200,200,404,500,500,500 -> 200:2, 404:1, 500:3")]
    public async Task ResponseCodesAreCounted()
    {
        var statistics = await GetStatistics("reference-codes.log");

        statistics.StatusDistribution.Count.Should().Be(3);
        statistics.StatusDistribution[(HttpStatusCode)200].Should().Be(2);
        statistics.StatusDistribution[(HttpStatusCode)404].Should().Be(1);
        statistics.StatusDistribution[(HttpStatusCode)500].Should().Be(3);
    }

    [Fact(DisplayName = "Top-10 ресурсов отсортирован по убыванию количества")]
    public async Task TopResourcesAreSortedDescending()
    {
        var statistics = await GetStatistics("access-2025-03-01.log");

        statistics.TopResources
            .Should().BeInDescendingOrder(pair => pair.Value);
    }

    [Fact(DisplayName = "Размеры 100..500: average 300, max 500, p95 500")]
    public async Task ResponseSizeStatistics()
    {
        var statistics = await GetStatistics("reference-sizes.log");

        statistics.AverageResponseSize.Should().Be(300);
        statistics.MaxResponseSize.Should().Be(500);
        statistics.P95responseSize.Should().Be(500);
    }

    [Fact(DisplayName = "Выбранный метод расчёта p95 зафиксирован и воспроизводим")]
    public async Task P95MethodIsDocumentedAndStable()
    {
        var firstStatistics = await GetStatistics("access-2025-03-01.log");
        var secondStatistics = await GetStatistics("access-2025-03-01.log");

        firstStatistics.P95responseSize
            .Should().Be(secondStatistics.P95responseSize);
    }

    private async Task<AnalysisStatistics> GetStatistics(string path)
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, path),
            OutputFormat: "json"
        );

        var result = await LogAnalyzer.AnalyzeAsync(options);

        return result.Statistics;
    }
}