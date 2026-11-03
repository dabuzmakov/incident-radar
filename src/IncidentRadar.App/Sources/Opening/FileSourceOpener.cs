namespace IncidentRadar.App.Sources.Opening;

public class FileSourceOpener : ISourceOpener
{
    public Task<Stream> OpenAsync(LogSource source)
        => Task.FromResult<Stream>(File.OpenRead(source.Path));
}
