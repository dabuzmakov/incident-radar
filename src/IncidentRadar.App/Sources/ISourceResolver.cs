namespace IncidentRadar.App.Sources;

public interface ISourceResolver
{
    IReadOnlyList<LogSource> GetSources(string path);
}
