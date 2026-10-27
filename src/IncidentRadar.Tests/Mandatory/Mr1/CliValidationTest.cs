using FluentAssertions;

namespace IncidentRadar.Tests.Mandatory.Mr1;

/// <summary>Обязательные тесты: валидация аргументов командной строки.</summary>
public class CliValidationTest
{
    [Fact(DisplayName = "Не передан обязательный --path -> exit code 2")]
    public async Task MissingPathExitsWithTwo()
    {
        var args = new string[] { "--output-format", "json" };
        var exitCode = await Program.Main(args);

        exitCode.Should().Be(2);
    }

    [Theory(DisplayName = "Неподдерживаемый --output-format -> exit code 2")]
    [InlineData("xml")]
    [InlineData("csv")]
    [InlineData("txt")]
    public async Task UnsupportedOutputFormatExitsWithTwo(string format)
    {
        var args = new string[] { "--path", "exampleName", "--output-format", $"{format}" };
        var exitCode = await Program.Main(args);

        exitCode.Should().Be(2);
    }

    [Theory(DisplayName = "--from больше или равен --to -> exit code 2")]
    [InlineData("2026-01-01", "2026-01-01")]
    [InlineData("2027-01-01", "2026-01-01")]
    public async Task FromNotBeforeToExitsWithTwo(string from, string to)
    {
        var args = new string[] { "--path", "exampleName", "--output-format", "json", "--from", $"{from}", "--to", $"{to}" };
        var exitCode = await Program.Main(args);

        exitCode.Should().Be(2);
    }

    [Fact(DisplayName = "Несуществующий локальный файл -> exit code 2")]
    public async Task MissingLocalFileExitsWithTwo()
    {
        var name = "bd7we86w78d6gh.txt";
        var args = new string[] { "--path", $"{name}", "--output-format", "json" };
        var exitCode = await Program.Main(args);

        exitCode.Should().Be(2);
    }

    [Fact(DisplayName = "Расширение файла отчёта не соответствует формату -> exit code 2")]
    public async Task OutputExtensionMismatchExitsWithTwo()
    {
        var outputPath = "report.md";
        var args = new string[] { "--path", "exampleName", "--output-format", "json", "--output-path", $"{outputPath}" };
        var exitCode = await Program.Main(args);

        exitCode.Should().Be(2);
    }
}