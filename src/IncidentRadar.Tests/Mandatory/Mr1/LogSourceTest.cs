namespace IncidentRadar.Tests.Mandatory.Mr1;

/// <summary>Обязательные тесты: источники логов.</summary>
public class LogSourceTest
{
    [Fact(DisplayName = "Локальный файл читается",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void ReadsLocalFile()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Glob-маска: в отчёт попадают все подошедшие файлы",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void ReadsAllFilesMatchingGlob()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Лог читается по URL",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void ReadsRemoteFile()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "URL вернул 404 -> exit code 2",
          Skip = "MR1: реализуй тест и удали эту строку")]
    public void Remote404ExitsWithTwo()
    {
        Assert.Fail("Тест не реализован");
    }
}