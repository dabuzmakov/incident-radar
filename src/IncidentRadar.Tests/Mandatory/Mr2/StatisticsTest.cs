namespace IncidentRadar.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: подсчёт статистики.</summary>
public class StatisticsTest
{
    [Fact(DisplayName = "Распределение кодов: 200,200,404,500,500,500 -> 200:2, 404:1, 500:3",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void ResponseCodesAreCounted()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Top-10 ресурсов отсортирован по убыванию количества",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void TopResourcesAreSortedDescending()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Размеры 100..500: average 300, max 500, p95 500",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void ResponseSizeStatistics()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Дробные значения округляются до 2 знаков после запятой",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void FractionalValuesAreRoundedToTwoDecimals()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Выбранный метод расчёта p95 зафиксирован и воспроизводим",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void P95MethodIsDocumentedAndStable()
    {
        Assert.Fail("Тест не реализован");
    }
}