namespace IncidentRadar.App.Logging;

public class ConsoleLogger : ILogger
{
    public void LogWarning(string message)
    {
        Console.Error.WriteLine($"WARN: {message}");
    }

    public void LogError(string message)
    {
        Console.Error.WriteLine($"ERROR: {message}");
    }
}