using System.CommandLine;

public class Program
{
    public static int Main(string[] args)
    {
        var pathOption = new Option<string>("--path", "-p")
        {
            Description = "Path to log file, glob pattern or URL",
            Required = true
        };

        var formatOption = new Option<string>("--output-format", "-f")
        {
            Description = "Report format",
            Required = true
        };

        formatOption.AcceptOnlyFromAmong("json", "markdown");

        var outputOption = new Option<string?>("--output-path", "-o")
        {
            Description = "Path to output report"
        };

        var fromOption = new Option<DateOnly?>("--from")
        {
            Description = "Start date"
        };

        var toOption = new Option<DateOnly?>("--to")
        {
            Description = "End date"
        };

        var rootCommand = new RootCommand(
            "Анализ NGINX access-логов и генерация отчётов");

        rootCommand.Options.Add(pathOption);
        rootCommand.Options.Add(formatOption);
        rootCommand.Options.Add(outputOption);
        rootCommand.Options.Add(fromOption);
        rootCommand.Options.Add(toOption);

        rootCommand.SetAction(parseResult =>
        {
            var path = parseResult.GetValue(pathOption);
            var format = parseResult.GetValue(formatOption);
            var output = parseResult.GetValue(outputOption);
            var from = parseResult.GetValue(fromOption);
            var to = parseResult.GetValue(toOption);

            Console.WriteLine($"Path: {path}");
            Console.WriteLine($"Format: {format}");
            Console.WriteLine($"Output: {output}");
            Console.WriteLine($"From: {from}");
            Console.WriteLine($"To: {to}");

            return 0;
        });

        return rootCommand.Parse(args).Invoke();
    }
}