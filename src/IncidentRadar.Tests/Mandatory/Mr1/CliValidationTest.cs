namespace IncidentRadar.Tests.Mandatory.Mr1;

/// <summary>Обязательные тесты: валидация аргументов командной строки.</summary>
public class CliValidationTest
{
    [Fact(DisplayName = "Не передан обязательный --path -> exit code 2",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void MissingPathExitsWithTwo()
    {
        Assert.Fail("Тест не реализован");
    }

    [Theory(DisplayName = "Неподдерживаемый --output-format -> exit code 2",
            Skip = "MR1: реализуй тест и удали эту строку")]
    [InlineData("xml")]
    [InlineData("csv")]
    [InlineData("txt")]
    public void UnsupportedOutputFormatExitsWithTwo(string format)
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "--from больше или равен --to -> exit code 2",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void FromNotBeforeToExitsWithTwo()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Несуществующий локальный файл -> exit code 2",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void MissingLocalFileExitsWithTwo()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Расширение файла отчёта не соответствует формату -> exit code 2",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void OutputExtensionMismatchExitsWithTwo()
    {
        Assert.Fail("Тест не реализован");
    }
}