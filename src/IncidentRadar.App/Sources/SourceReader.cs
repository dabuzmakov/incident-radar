namespace IncidentRadar.App.Sources;

public class SourceReader
{
    private readonly SourceResolver _resolver;

    public SourceReader(SourceResolver resolver)
    {
        _resolver = resolver;
    }

    public async IAsyncEnumerable<string> ReadAsync()
    {
        foreach (var source in _resolver.GetSources())
        {
            using var stream = await _resolver.OpenAsync(source);
            using var reader = new StreamReader(stream);    

            while (await reader.ReadLineAsync() is string line)
                yield return line;
        }
    }
}
