using IncidentRadar.App;
using IncidentRadar.App.Analysis;
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
            return await RunAsync(app, parseResult.Options!);
        }
        catch (SourceException exсeption)
        {
            app.Logger.LogError($"Ошибка источника, {exсeption.Message}");
            return (int)ExitCode.InvalidInput;
        }
        catch (OutputException exсeption)
        {
            app.Logger.LogError($"Ошибка выходного файла, {exсeption.Message}");
            return (int)ExitCode.InvalidInput;
        }
        catch (Exception exсeption)
        {
            app.Logger.LogError($"Внутренняя ошибка, {exсeption.Message}");
            return (int)ExitCode.InternalError;
        }
    }

    private static async Task<int> RunAsync(AppComposer app, AnalysisOptions options)
    {
        var outputPath = options.OutputPath;
        var overwrite = false;

        if (outputPath is not null)
        {
            outputPath = app.ReportSaver.PreparePath(outputPath);

            if (File.Exists(outputPath) && !app.UI.ConfirmOverwrite(outputPath))
            {
                return (int)ExitCode.InvalidInput;
            }

            overwrite = File.Exists(outputPath);
        }

        var result = await app.Analyzer.AnalyzeAsync(options);
        var builder = app.ReportBuilderFactory.GetReportBuilder(options.OutputFormat!);
        var report = builder.CreateReport(result);

        if (outputPath is null)
        {
            app.UI.WriteLine(report);
        }
        else
        {
            app.ReportSaver.Save(report, outputPath, overwrite);
        }

        return (int)ExitCode.Success;
    }
}