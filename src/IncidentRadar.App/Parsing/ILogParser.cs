namespace IncidentRadar.App.Parsing;

public interface ILogParser
{
    bool TryParse(string line, out LogEntry entry);
}
