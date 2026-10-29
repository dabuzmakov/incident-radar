using FluentAssertions;
using IncidentRadar.App.Analysis;
using IncidentRadar.App.Parsing;

namespace IncidentRadar.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: разбор строки лога.</summary>
public class LogParsingTest
{
    private static readonly string _samplesDirectory
        = Path.Combine(AppContext.BaseDirectory, "samples/logs");
    private static readonly string _referenceLog 
        = "91.234.194.89 - - [01/Mar/2025:00:10:02 +0000] \"GET /health HTTP/1.1\" 304 0 \"https://www.tbank.ru/\" \"Debian APT-HTTP/1.3 (2.6.1)\"";

    [Fact(DisplayName = "Эталонная строка разбирается на корректные поля")]
    public void ReferenceLineIsParsedCorrectly()
    {
        LogParser.TryParse(_referenceLog, out _).Should().BeTrue();
    }

    [Fact(DisplayName = "Метод, ресурс и протокол разбираются в три отдельных поля")]
    public void RequestIsSplitIntoThreeFields()
    {
        if (!LogParser.TryParse(_referenceLog, out var logEntry))
            Assert.Fail();

        var request = logEntry.Request;

        request.Method.Method.Should().Be("GET");
        request.Resource.Should().Be("/health");
        request.Protocol.Should().Be("HTTP/1.1");
    }

    [Fact(DisplayName = "Битая строка пропускается с warning'ом, программа не падает")]
    public async Task MalformedLineIsSkippedWithWarning()
    {
        var options = new AnalysisOptions
        (
            Path: Path.Combine(_samplesDirectory, "malformed.log"),
            OutputFormat: "json"
        );

        var result = await LogAnalyzer.AnalyzeAsync(options);

        result.TotalRead.Should().Be(13);
        result.MalformedCount.Should().Be(6);
    }
}