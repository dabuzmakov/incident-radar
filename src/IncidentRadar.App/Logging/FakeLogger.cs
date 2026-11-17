namespace IncidentRadar.App.Logging;

public class FakeLogger : ILogger
{
    public List<string> Warnings { get; } = new();
    public List<string> Errors { get; } = new();

    public void LogWarning(string message) => Warnings.Add(message);

    public void LogError(string message) => Errors.Add(message);
}
