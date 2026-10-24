using IncidentRadar.App.Analysis;
using System.Globalization;

namespace IncidentRadar.App.CLI;

public static class CommandParser
{
    private static Dictionary<string, string> _supportedFormats = new() 
    { 
        ["json"] = ".json",
        ["markdown"] = ".md" 
    };

    private static HashSet<string> _knownOptions = new()
    {
        "--path",
        "--output-format",
        "--output-path",
        "--from",
        "--to"
    };

    private static Dictionary<string, string> _aliases = new()
    {
        ["-p"] = "--path",
        ["-f"] = "--output-format",
        ["-o"] = "--output-path"
    };

    public static ParseResult Parse(string[] args)
    {
        var errors = new List<string>();

        var (values, argumentErrors) = ParseArguments(args);
        errors.AddRange(argumentErrors);

        errors.AddRange(ValidateRequiredOptions(values));

        values.TryGetValue("--path", out var path);
        values.TryGetValue("--output-format", out var format);
        values.TryGetValue("--output-path", out var outputPath);

        var (from, fromErrors) = ParseDate(values, "--from");
        var (to, toErrors) = ParseDate(values, "--to");

        errors.AddRange(fromErrors);
        errors.AddRange(toErrors);

        if (from.HasValue && to.HasValue && from >= to)
            errors.Add("Дата --from должна быть раньше чем --to.");

        errors.AddRange(ValidateOutputPath(outputPath, format));

        var options = errors.Count == 0
            ? new AnalysisOptions(path!, format!, outputPath, from, to)
            : null;

        return new ParseResult(options, errors);
    }

    private static IEnumerable<string> ValidateRequiredOptions(IReadOnlyDictionary<string, string> values)
    {
        if (!values.TryGetValue("--path", out var path) || string.IsNullOrWhiteSpace(path))
            yield return "--path является обязательным аргументом.";

        if (!values.TryGetValue("--output-format", out var format))
            yield return "--output-format является обязательным аргументом.";
        else if (!_supportedFormats.ContainsKey(format))
            yield return $"Выходной формат {format} не поддерживается.";
    }

    private static IEnumerable<string> ValidateOutputPath(string? outputPath, string? format)
    {
        if (outputPath is null)
            yield break;

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            yield return "--output-path не может быть пустым.";
            yield break;
        }

        if (format is null || !_supportedFormats.TryGetValue(format, out var extension))
            yield break;

        if (!string.Equals(Path.GetExtension(outputPath), extension, StringComparison.OrdinalIgnoreCase))
            yield return $"Формат выходного файла должен иметь расширение {extension} .";
    }

    private static (DateOnly? Date, string[] Errors) ParseDate(IReadOnlyDictionary<string, string> values, string option)
    {
        if (!values.TryGetValue(option, out var value))
            return (null, []);

        if (DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return (date, []);

        return (null, [$"Некорректный формат даты {option}. Ожидается: yyyy-MM-dd."]);
    }

    private static (Dictionary<string, string> Values, List<string> Errors) ParseArguments(string[] args)
    {
        var values = new Dictionary<string, string>();
        var errors = new List<string>();

        for (var i = 0; i < args.Length; i++)
        {
            var option = args[i];

            if (_aliases.TryGetValue(option, out var fullName))
                option = fullName;

            if (!_knownOptions.Contains(option))
            {
                errors.Add($"Неизвестный аргумент: {args[i]}.");
                continue;
            }

            if (i + 1 >= args.Length || args[i + 1].StartsWith('-'))
            {
                errors.Add($"Пропущено значение аргумента {option}.");
                continue;
            }

            if (!values.TryAdd(option, args[++i]))
                errors.Add($"Повторный аргумент: {option}.");
        }

        return (values, errors);
    }
}
