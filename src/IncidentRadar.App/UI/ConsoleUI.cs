namespace IncidentRadar.App.UI;

public class ConsoleUI : IApplicationUI
{
    public bool ConfirmOverwrite(string path)
    {
        WriteLine($"Файл \"{path}\" уже существует, перезаписать? [y/N]: ");

        var answer = Console.ReadLine();

        return string.Equals(
            answer?.Trim(),
            "y",
            StringComparison.OrdinalIgnoreCase);

    }

    public void WriteLine(string text)
        => Console.WriteLine(text);
}
