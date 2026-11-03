using IncidentRadar.App.Analysis;

namespace IncidentRadar.App.CLI;

public class OptionsValidator
{
    private readonly IReadOnlyDictionary<string, string> _supportedFormats;

    public OptionsValidator(IReadOnlyDictionary<string, string> supportedFormats)
    {
        _supportedFormats = supportedFormats;
    }

    public IEnumerable<string> Validate(AnalysisOptions options)
    {
        foreach (var error in ValidateRequiredOptions(options))
            yield return error;

        foreach (var error in ValidateOutputPath(options))
            yield return error;

        foreach (var error in ValidateDates(options))
            yield return error;
    }

    private IEnumerable<string> ValidateRequiredOptions(AnalysisOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Path))
        {
            yield return "--path является обязательным аргументом.";
        }

        if (string.IsNullOrWhiteSpace(options.OutputFormat))
        {
            yield return "--output-format является обязательным аргументом.";
        }
        else if (!_supportedFormats.ContainsKey(options.OutputFormat))
        {
            yield return $"Выходной формат {options.OutputFormat} не поддерживается.";
        }
    }

    private IEnumerable<string> ValidateOutputPath(AnalysisOptions options)
    {
        var outputPath = options.OutputPath;
        var format = options.OutputFormat;

        if (outputPath is null)
        {
            yield break;
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            yield return "--output-path не может быть пустым.";
            yield break;
        }

        if (format is null || !_supportedFormats.TryGetValue(format, out var extension))
        {
            yield break;
        }

        if (!string.Equals(Path.GetExtension(outputPath), extension, StringComparison.OrdinalIgnoreCase))
        {
            yield return $"Формат выходного файла должен иметь расширение {extension} .";
        }
    }

    private IEnumerable<string> ValidateDates(AnalysisOptions options)
    {
        if (options.From is { } from &&
            options.To is { } to &&
            from > to)
        {
            yield return "Дата --from должна быть не позже --to.";
        }
    }
}
