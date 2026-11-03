namespace IncidentRadar.App.Sources.Opening;

public class UrlSourceOpener : ISourceOpener
{
    private static readonly HttpClient _httpClient = new();

    public Task<Stream> OpenAsync(LogSource source)
        => _httpClient.GetStreamAsync(source.Path);
}
