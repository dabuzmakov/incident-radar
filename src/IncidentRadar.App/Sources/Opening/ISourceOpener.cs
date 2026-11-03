namespace IncidentRadar.App.Sources.Opening;

public interface ISourceOpener
{
    Task<Stream> OpenAsync(LogSource source);
}
