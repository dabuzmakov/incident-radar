namespace IncidentRadar.App.UI;

public class ConsoleUI : IApplicationUI
{
    public bool ConfirmOverwrite(string path)
    {
        Write($"Файл \"{path}\" уже существует, перезаписать? [y/N]: ");

        var answer = Console.ReadLine();

        return string.Equals(
            answer?.Trim(),
            "y",
            StringComparison.OrdinalIgnoreCase);

    }

    public void Write(string text)
        => Console.Write(text);

    public void WriteLine(string text)
        => Console.WriteLine(text);
}
