using IncidentRadar.App.Analysis;
using System.Globalization;

namespace IncidentRadar.App.CLI;

public class CommandParser
{
    private readonly OptionsValidator _validator;

    private static readonly HashSet<string> _knownOptions = new()
    {
        "--path",
        "--output-format",
        "--output-path",
        "--from",
        "--to"
    };

    private static readonly Dictionary<string, string> _aliases = new()
    {
        ["-p"] = "--path",
        ["-f"] = "--output-format",
        ["-o"] = "--output-path"
    };

    public CommandParser(OptionsValidator validator)
    {
        _validator = validator;
    }

    public ParseResult Parse(string[] args)
    {
        var errors = new List<string>();

        var (optionsDictionary, argumentErrors) = ParseArguments(args);

        optionsDictionary.TryGetValue("--path", out var path);
        optionsDictionary.TryGetValue("--output-format", out var format);
        optionsDictionary.TryGetValue("--output-path", out var outputPath);

        var (from, fromErrors) = ParseDate(optionsDictionary, "--from");
        var (to, toErrors) = ParseDate(optionsDictionary, "--to");

        var options = new AnalysisOptions(path, format, outputPath, from, to);

        errors.AddRange(argumentErrors);
        errors.AddRange(fromErrors);
        errors.AddRange(toErrors);
        errors.AddRange(_validator.Validate(options));

        return new ParseResult(
            errors.Count == 0 ? options : null,
            errors
        );
    }

    private static (DateOnly? Date, string[] Errors) ParseDate(IReadOnlyDictionary<string, string> values, string option)
    {
        if (!values.TryGetValue(option, out var value))
        {
            return (null, []);
        }

        if (DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return (date, []);
        }

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
            {
                option = fullName;
            }

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
            {
                errors.Add($"Повторный аргумент: {option}.");
            }
        }

        return (values, errors);
    }
}
