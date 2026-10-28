using FluentAssertions;
using IncidentRadar.App.Analysis;
using IncidentRadar.App.Sources;

namespace IncidentRadar.Tests.Mandatory.Mr1;

/// <summary>Обязательные тесты: источники логов.</summary>
public class LogSourceTest
{
    private static string _samplesDirectory
        = Path.Combine(AppContext.BaseDirectory, "samples/logs");

    private static string _sampleUrl 
        = "https://raw.githubusercontent.com/elastic/examples/master/Common%20Data%20Formats/nginx_logs/nginx_logs";

    [Theory(DisplayName = "Локальный файл читается")]
    [InlineData("access-2025-03-01.log")]
    [InlineData("access-2025-03-02.log")]
    public async Task ReadsLocalFile(string fileName)
    {
        var file = Path.Combine(_samplesDirectory, fileName);

        var options = new AnalysisOptions
        (
            Path: file,
            OutputFormat: "json"
        );

        var result = await LogAnalyzer.AnalyzeAsync(options);

        result.TotalRead.Should().BeGreaterThan(0);
    }

    public static TheoryData<string[], string> LogFiles = new()
    {
        { ["access-2025-03-01.log", "access-2025-03-02.log", "malformed.log", "reference-codes.log", "reference-sizes.log"], "*.log" },
        { ["access-2025-03-01.log"], "*1.log" },
        { ["access-2025-03-01.log", "access-2025-03-02.log"], "access-????-??-??.log" }
    };

    [Theory(DisplayName = "Glob-маска: попадают все подошедшие файлы")]
    [MemberData(nameof(LogFiles))]
    public void ReadsAllFilesMatchingGlob(string[] fileNames, string mask)
    {
        var path = Path.Combine(_samplesDirectory, mask);
        var sources = SourceResolver.GetSources(path);

        var names = sources.Select(source => Path.GetFileName(source.Path));
        names.Should().BeEquivalentTo(fileNames);
    }

    [Fact(DisplayName = "Лог читается по URL")]
    public async Task ReadsRemoteFile()
    {
        var options = new AnalysisOptions
        (
            Path: _sampleUrl,
            OutputFormat: "json"
        );

        var result = await LogAnalyzer.AnalyzeAsync(options);

        result.TotalRead.Should().BeGreaterThan(0);
    }

    [Fact(DisplayName = "URL вернул 404 -> exit code 2")]
    public async Task Remote404ExitsWithTwo()
    {
        var badUrl = _sampleUrl + "bas76dgasd9a";
        var args = new string[] { "--path", $"{badUrl}", "--output-format", "json" };
        var exitCode = await Program.Main(args);

        exitCode.Should().Be(2);
    }
}