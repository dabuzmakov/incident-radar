namespace IncidentRadar.App.Sources;

public class SourceResolver
{
    private readonly string _path;

    public SourceResolver(string Path)
    {
        _path = Path;
    }

    public IEnumerable<LogSource> GetSources()
    {
        throw new NotImplementedException();
    }

    public async Task<Stream> OpenAsync(LogSource source)
    {
        return source.Type switch
        {
            SourceType.File => File.OpenRead(source.Path),
            SourceType.Url => await new HttpClient().GetStreamAsync(source.Path),
            _ => throw new ArgumentOutOfRangeException(nameof(source.Type))
        };
    }
}
