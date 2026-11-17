namespace IncidentRadar.App.Sources.Resolving;

public interface ISourceResolver
{
    IReadOnlyList<LogSource> GetSources(string path);
}
