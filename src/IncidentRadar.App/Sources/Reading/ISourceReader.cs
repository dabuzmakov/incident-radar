namespace IncidentRadar.App.Sources.Reading;

public interface ISourceReader
{
    IAsyncEnumerable<string> ReadAsync();
}
