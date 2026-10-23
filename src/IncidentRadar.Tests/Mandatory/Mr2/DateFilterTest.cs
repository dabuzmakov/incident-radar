namespace IncidentRadar.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: фильтрация по датам.</summary>
public class DateFilterTest
{
    [Fact(DisplayName = "Без --from и --to обрабатываются все записи",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void NoFilterKeepsEverything()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Только --from: открытая правая граница",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void OnlyFromIsOpenEnded()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Только --to: открытая левая граница",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void OnlyToIsOpenEnded()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Обе границы: период включительно",
          Skip = "MR2: реализуй тест и удали эту строку")]
    public void BothBoundsAreInclusive()
    {
        Assert.Fail("Тест не реализован");
    }
}