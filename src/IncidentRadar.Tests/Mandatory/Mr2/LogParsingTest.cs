namespace IncidentRadar.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: разбор строки лога.</summary>
public class LogParsingTest
{
    [Fact(DisplayName = "Эталонная строка разбирается на корректные поля",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void ReferenceLineIsParsedCorrectly()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Метод, ресурс и протокол разбираются в три отдельных поля",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void RequestIsSplitIntoThreeFields()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Битая строка пропускается с warning'ом, программа не падает",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void MalformedLineIsSkippedWithWarning()
    {
        Assert.Fail("Тест не реализован");
    }
}