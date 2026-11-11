namespace IncidentRadar.App.UI;

public class ConsoleUI : IApplicationUI
{
    public bool ConfirmOverwrite()
    {
        WriteLine("Файл уже существует, перезаписать? [y/N]: ");

        var answer = Console.ReadLine();

        return string.Equals(
            answer?.Trim(),
            "y",
            StringComparison.OrdinalIgnoreCase);

    }

    public void WriteLine(string text)
        => Console.WriteLine(text);
}
