using IncidentRadar.App.Exceptions;

namespace IncidentRadar.App.Sources;

public class SourceReader
{
    private readonly IReadOnlyList<LogSource> _sources;

    public SourceReader(IReadOnlyList<LogSource> sources)
    {
        _sources = sources;
    }

    public async IAsyncEnumerable<string> ReadAsync()
    {
        foreach (var source in _sources)
        {
            using var stream = await SourceResolver.OpenAsync(source);
            using var reader = new StreamReader(stream);

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
                    break;

                yield return line;
            }
        }
    }
}
