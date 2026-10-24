using System.CommandLine;

namespace IncidentRadar.App.CLI;

public static class CommandBuilder
{
    public static RootCommand CreateRoot()
    {
        var rootCommand = new RootCommand(
            "Анализ NGINX access-логов и генерация отчётов");

        foreach (var option in CreateOptions())
            rootCommand.Options.Add(option);

        return rootCommand;
    }

    private static IEnumerable<Option> CreateOptions()
    {
        var pathOption = new Option<string>("--path", "-p") { Required = true };
        var formatOption = new Option<string>("--output-format", "-f") { Required = true };
        var outputOption = new Option<string?>("--output-path", "-o");
        var fromOption = new Option<DateOnly?>("--from");
        var toOption = new Option<DateOnly?>("--to");

        formatOption.AcceptOnlyFromAmong("json", "markdown");

        pathOption.Description = "Path to log file, glob pattern or URL";
        formatOption.Description = "Report format";
        outputOption.Description = "Path to output report";
        fromOption.Description = "Start date";
        toOption.Description = "End date";

        return [pathOption, formatOption, outputOption, fromOption, toOption];
    }
}
