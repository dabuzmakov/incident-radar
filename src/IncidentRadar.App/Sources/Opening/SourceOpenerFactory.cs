namespace IncidentRadar.App.Sources.Opening;

public class SourceOpenerFactory
{
    private readonly ISourceOpener _fileOpener;
    private readonly ISourceOpener _urlOpener;

    public SourceOpenerFactory(
        FileSourceOpener fileOpener,
        UrlSourceOpener urlOpener)
    {
        _fileOpener = fileOpener;
        _urlOpener = urlOpener;
    }

    public ISourceOpener GetOpener(SourceType type)
    {
        return type switch
        {
            SourceType.File => _fileOpener,
            SourceType.Url => _urlOpener,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
