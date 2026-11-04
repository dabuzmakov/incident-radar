using IncidentRadar.App.Exceptions;
using IncidentRadar.App.Sources.Opening;

namespace IncidentRadar.App.Sources.Reading;

public class SourceReader : ISourceReader
{
    private readonly IReadOnlyList<LogSource> _sources;
    private readonly SourceOpenerFactory _openerFactory;

    public SourceReader(
        IReadOnlyList<LogSource> sources, 
        SourceOpenerFactory openerFactory)
    {
        _sources = sources;
        _openerFactory = openerFactory;
    }

    public async IAsyncEnumerable<string> ReadAsync()
    {
        foreach (var source in _sources)
        {
            await foreach (var line in ReadSource(source))
            {
                yield return line;
            }
        }
    }

    private async IAsyncEnumerable<string> ReadSource(LogSource source)
    {
        using var reader = await OpenReaderAsync(source);

        while (true)
        {
            string? line;

            try
            {
                line = await reader.ReadLineAsync();
            }
            catch (Exception exception)
            {
                throw new SourceException(
                    $"Не удалось прочитать источник {source.Path}", exception);
            }

            if (line is null)
            {
                yield break;
            }

            yield return line;
        }
    }

    private async Task<StreamReader> OpenReaderAsync(LogSource source)
    {
        try
        {
            var opener = _openerFactory.GetOpener(source.Type);
            using var stream = await opener.OpenAsync(source);

            return new StreamReader(stream);
        }
        catch (Exception exception)
        {
            throw new SourceException(
                $"Не удалось открыть источник {source.Path}", exception);
        }
    }
}
