using FluentAssertions;
using IncidentRadar.App.Analysis;

namespace IncidentRadar.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: фильтрация по датам.</summary>
public class DateFilterTest
{
    private static readonly string _samplesDirectory
        = Path.Combine(AppContext.BaseDirectory, "samples/logs");
    private static readonly Task<AnalysisResult> _firstMarResult
        = GetResult("access-2025-03-01.log");
    private static readonly Task<AnalysisResult> _secondMarResult
        = GetResult("access-2025-03-02.log");

    [Theory(DisplayName = "Без --from и --to обрабатываются все записи")]
    [InlineData("access-2025-03-01.log")]
    [InlineData("access-2025-03-02.log")]
    public async Task NoFilterKeepsEverything(string fileName)
    {
        var fullPath = Path.Combine(_samplesDirectory, fileName);
        var count = File.ReadLines(fullPath).Count();

        var result = await GetResult(fileName);

        result.TotalRead.Should().Be(count);
    }

    [Fact(DisplayName = "Только --from: открытая правая граница")]
    public async Task OnlyFromIsOpenEnded()
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, "access-2025-03-??.log"),
            OutputFormat: "json",
            From: new DateOnly(2025, 03, 02)
        );

        var resultWithFrom = await LogAnalyzer.AnalyzeAsync(options);
        var targetFileResult = await _secondMarResult;

        resultWithFrom.Statistics.RequestsCount
            .Should().Be(targetFileResult.Statistics.RequestsCount);
    }

    [Fact(DisplayName = "Только --to: открытая левая граница")]
    public async Task OnlyToIsOpenEnded()
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, "access-2025-03-??.log"),
            OutputFormat: "json",
            To: new DateOnly(2025, 03, 01)
        );

        var resultWithTo = await LogAnalyzer.AnalyzeAsync(options);
        var targetFileResult = await _firstMarResult;

        resultWithTo.Statistics.RequestsCount
            .Should().Be(targetFileResult.Statistics.RequestsCount);
    }

    [Fact(DisplayName = "Обе границы: период включительно")]
    public async Task BothBoundsAreInclusive()
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, "access-2025-03-??.log"),
            OutputFormat: "json",
            From: new DateOnly(2025, 03, 01),
            To: new DateOnly(2025, 03, 02)
        );

        var result = await LogAnalyzer.AnalyzeAsync(options);
        var firstFileResult = await _firstMarResult;
        var secondFileResult = await _secondMarResult;

        result.Statistics.RequestsCount
            .Should().Be(
                firstFileResult.Statistics.RequestsCount +
                secondFileResult.Statistics.RequestsCount);
    }

    private static async Task<AnalysisResult> GetResult(string fileName)
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, fileName),
            OutputFormat: "json"
        );

        return await LogAnalyzer.AnalyzeAsync(options);
    }
}