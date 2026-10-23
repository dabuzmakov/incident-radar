namespace IncidentRadar.Tests.Mandatory.Mr3;

/// <summary>Обязательные тесты: отчёты и коды возврата — blackbox-проверки CLI.</summary>
public class ReportAndExitCodeTest
{
    [Fact(DisplayName = "JSON-отчёт валиден и содержит все обязательные поля",
          Skip = "MR3: реализуй тест и удали эту строку")]
    public void JsonReportIsValidAndComplete()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Markdown-отчёт содержит заголовок и таблицы",
          Skip = "MR3: реализуй тест и удали эту строку")]
    public void MarkdownReportHasHeaderAndTables()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Существующий файл отчёта не перезаписывается -> ошибка",
          Skip = "MR3: реализуй тест и удали эту строку")]
    public void ExistingOutputFileIsNotOverwritten()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Успешный запуск -> exit code 0",
          Skip = "MR3: реализуй тест и удали эту строку")]
    public void SuccessfulRunExitsWithZero()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Некорректное использование -> exit code 2, а не 1",
          Skip = "MR3: реализуй тест и удали эту строку")]
    public void MisuseExitsWithTwoNotOne()
    {
        Assert.Fail("Тест не реализован");
    }
}