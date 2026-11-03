namespace IncidentRadar.App.Sources;

public interface ISourceReader
{
    IAsyncEnumerable<string> ReadAsync();
}
