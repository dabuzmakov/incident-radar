using FluentAssertions;
using IncidentRadar.App;
using IncidentRadar.App.Analysis;
using IncidentRadar.App.Analysis.Statistics;
using IncidentRadar.App.CLI;
using IncidentRadar.App.Logging;
using IncidentRadar.App.Reports;
using IncidentRadar.App.Sources;
using IncidentRadar.App.UI;
using System.Net;
using System.Text.Json;

namespace IncidentRadar.Tests.Mandatory.Mr3;

/// <summary>Обязательные тесты: отчёты и коды возврата — blackbox-проверки CLI.</summary>
public class ReportAndExitCodeTest
{
    private static readonly AppComposer _app = new();

    private static readonly string _samplesDirectory
        = Path.Combine(AppContext.BaseDirectory, "samples/logs");

    private static readonly AnalysisResult _result = new(
        Statistics: new AnalysisStatistics(
            RequestsCount: 100,
            StatusDistribution: new Dictionary<HttpStatusCode, int>
            {
                [HttpStatusCode.OK] = 70,
                [HttpStatusCode.NotFound] = 20,
                [HttpStatusCode.InternalServerError] = 10
            },
            TopResources:
            [
                new("/api/users", 50),
                new("/index.html", 30),
                new("/api/products", 20)
            ],
            Protocols: ["HTTP/1.1", "HTTP/2.0"],
            AverageResponseSize: 512.5,
            MaxResponseSize: 2048,
            P95responseSize: 1024
        ),
        Sources:
        [
            new LogSource("access.log", SourceType.File),
            new LogSource("errors.log", SourceType.File)
        ],
        TotalRead: 110,
        MalformedCount: 10);

    public static TheoryData<string[]> InvalidArguments => new()
    {
        new[] { "--output-format", "json" },
        new[] { "--path", "access.log" },
        new[] { "--path", "access.log", "--output-format", "xml" },
        new[]
        {
            "--path", "access.log",
            "--output-format", "json",
            "--output-path", "report.md"
        },
        new[]
        {
            "--path", "access.log",
            "--output-format", "json",
            "--from", "2025-03-02",
            "--to", "2025-03-01"
        },
        new[]
        {
            "--path", "access.log",
            "--output-format", "json",
            "--from", "не-дата"
        },
    };

    [Fact(DisplayName = "JSON-отчёт валиден и содержит все обязательные поля")]
    public void JsonReportIsValidAndComplete()
    {
        var reportBuilder = new JsonReportBuilder();
        var report = reportBuilder.CreateReport(_result);

        using var document = JsonDocument.Parse(report);
        var root = document.RootElement;

        root.GetProperty("files")
            .EnumerateArray()
            .Select(x => x.GetString())
            .Should().BeEquivalentTo(["access.log", "errors.log"]);

        root.GetProperty("filesCount").GetInt32().Should().Be(2);
        root.GetProperty("processedLinesCount").GetInt32().Should().Be(110);
        root.GetProperty("malformedLinesCount").GetInt32().Should().Be(10);
        root.GetProperty("totalRequestsCount").GetInt32().Should().Be(100);

        var sizes = root.GetProperty("responseSizeInBytes");

        sizes.GetProperty("average").GetDouble().Should().Be(512.5);
        sizes.GetProperty("max").GetInt64().Should().Be(2048);
        sizes.GetProperty("p95").GetInt64().Should().Be(1024);

        var resources = root.GetProperty("resources").EnumerateArray().ToArray();

        resources.Should().HaveCount(3);
        resources[0].GetProperty("resource").GetString().Should().Be("/api/users");
        resources[0].GetProperty("count").GetInt32().Should().Be(50);

        var codes = root.GetProperty("responseCodes").EnumerateArray().ToArray();

        codes.Should().HaveCount(3);
        codes[0].GetProperty("code").GetInt32().Should().Be(200);
        codes[0].GetProperty("name").GetString().Should().Be("OK");
        codes[0].GetProperty("count").GetInt32().Should().Be(70);

        codes[1].GetProperty("code").GetInt32().Should().Be(404);
        codes[1].GetProperty("name").GetString().Should().Be("NotFound");

        codes[2].GetProperty("code").GetInt32().Should().Be(500);
        codes[2].GetProperty("name").GetString().Should().Be("InternalServerError");

        root.GetProperty("protocols")
            .EnumerateArray()
            .Select(x => x.GetString())
            .Should().BeEquivalentTo(["HTTP/1.1", "HTTP/2.0"]);
    }

    [Fact(DisplayName = "Markdown-отчёт содержит заголовок и таблицы")]
    public void MarkdownReportHasHeaderAndTables()
    {
        var reportBuilder = new MarkdownReportBuilder();
        var report = reportBuilder.CreateReport(_result);

        report.Should().StartWith("# Incident Radar");

        report.Should().Contain("- Files count: 2");
        report.Should().Contain("- Processed lines: 110");
        report.Should().Contain("- Malformed lines: 10");
        report.Should().Contain("- Total requests: 100");

        report.Should().Contain("## Files");
        report.Should().Contain("- access.log");
        report.Should().Contain("- errors.log");

        report.Should().Contain("## Response size in bytes");
        report.Should().Contain("- Average: 512.50");
        report.Should().Contain("- Max: 2048");
        report.Should().Contain("- P95: 1024");

        report.Should().Contain("## Resources (top 10)");
        report.Should().Contain("| Resource | Count |");
        report.Should().Contain("| /api/users | 50 |");
        report.Should().Contain("| /index.html | 30 |");
        report.Should().Contain("| /api/products | 20 |");

        report.Should().Contain("## Response codes");
        report.Should().Contain("| Code | Name | Count |");
        report.Should().Contain("| 200 | OK | 70 |");
        report.Should().Contain("| 404 | NotFound | 20 |");
        report.Should().Contain("| 500 | InternalServerError | 10 |");

        report.Should().Contain("## Protocols");
        report.Should().Contain("- HTTP/1.1");
        report.Should().Contain("- HTTP/2.0");
    }

    [Fact(DisplayName = "Существующий файл отчёта не перезаписывается -> ошибка")]
    public void ExistingOutputFileIsNotOverwritten()
    {
        var path = Path.GetTempFileName();

        try
        {
            File.WriteAllText(path, "исходный отчет");

            var saver = new FileReportSaver();

            Action save = () => saver.Save(
                report: "новый отчет",
                path: path,
                overwrite: false);

            save.Should().Throw<IOException>();

            File.ReadAllText(path)
                .Should().Be("исходный отчет");
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Theory(DisplayName = "Успешный запуск -> exit code 0")]
    [InlineData("access-2025-03-01.log")]
    [InlineData("access-2025-03-02.log")]
    [InlineData("https://raw.githubusercontent.com/elastic/examples/master/Common%20Data%20Formats/nginx_logs/nginx_logs")]
    public async Task SuccessfulRunExitsWithZero(string name)
    {
        var app = new AppComposer(
            ui: new FakeUI(),
            logger: new FakeLogger()
            );

        var path = name[0..8] == "https://" 
            ? name
            : Path.Combine(_samplesDirectory, name);

        var options = new AnalysisOptions(Path: path, OutputFormat: "json");
        var code = await Program.RunAsync(app, options);

        code.Should().Be((int)ExitCode.Success);
    }

    [Theory(DisplayName = "Некорректное использование -> exit code 2, а не 1")]
    [MemberData(nameof(InvalidArguments))]
    public async Task MisuseExitsWithTwoNotOne(string[] args)
    {
        var code = await Program.Main(args);

        code.Should().Be((int)ExitCode.InvalidInput);
    }
}