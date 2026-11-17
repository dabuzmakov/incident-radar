namespace IncidentRadar.App.Logging;

public interface ILogger
{
    void LogWarning(string message);

    void LogError(string message);
}