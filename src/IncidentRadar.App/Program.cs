using IncidentRadar.App.CLI;
using System.CommandLine;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = CommandBuilder.CreateRoot();
        var parseResult = rootCommand.Parse(args);

        if (CanInvokeWithErrors(parseResult))
            return await parseResult.InvokeAsync();

        if (parseResult.Errors.Count > 0)
        {
            foreach (var error in parseResult.Errors)
                Console.WriteLine(error.Message);

            return (int)ExitCode.InvalidInput;
        }

        return (int)ExitCode.Success;
    }

    private static bool CanInvokeWithErrors(ParseResult result) 
        => result.Action?.ClearsParseErrors == true;
}