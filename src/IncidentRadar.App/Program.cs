using IncidentRadar.App;
using IncidentRadar.App.CLI;
using IncidentRadar.App.Exceptions;
using System.Text;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var app = new AppComposer();
        var parseResult = app.CommandParser.Parse(args);

        if (!parseResult.IsSuccess)
        {
            foreach (var error in parseResult.Errors)
            {
                app.Logger.LogError(error);
            }

            return (int)ExitCode.InvalidInput;
        }

        try
        {
            var format = parseResult.Options!.OutputFormat;
            var outputPath = parseResult.Options!.OutputPath;

            var reportBuilder = app.ReportBuilderFactory.GetReportBuilder(format!);

            var result = await app.Analyzer.AnalyzeAsync(parseResult.Options!);
            var report = reportBuilder.CreateReport(result);

            return (int)ExitCode.Success;
        }
        catch (SourceException exсeption)
        {
            app.Logger.LogError($"Ошибка источника, {exсeption.Message}");
            return (int)ExitCode.InvalidInput;
        }
        catch (Exception exсeption)
        {
            app.Logger.LogError($"Внутренняя ошибка, {exсeption.Message}");
            return (int)ExitCode.InternalError;
        }
    }
}