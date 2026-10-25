namespace IncidentRadar.App.Sources;

public class SourceResolver
{
    public IReadOnlyList<LogSource> Sources { get; }
    private static readonly HttpClient _httpClient = new();
    private readonly HashSet<string> _supportedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".log", ".txt" };

    public SourceResolver(string path)
    {
        Sources = GetSources(path);
    }

    public async Task<Stream> OpenAsync(LogSource source)
    {
        return source.Type switch
        {
            SourceType.File => File.OpenRead(source.Path),
            SourceType.Url => await _httpClient.GetStreamAsync(source.Path),
            _ => throw new ArgumentOutOfRangeException(nameof(source.Type))
        };
    }

    private IReadOnlyList<LogSource> GetSources(string path)
    {
        if (IsUrl(path))
            return ResolveUrl(path);

        if (IsGlob(path))
            return ResolveGlob(path);

        return ResolveFile(path);
    }

    private IReadOnlyList<LogSource> ResolveUrl(string path)
    {
        if (!Uri.TryCreate(path, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(uri.Host))
            throw new ArgumentException($"Некорректный URL: {path}");

        return [new LogSource(path, SourceType.Url)];
    }

    private IReadOnlyList<LogSource> ResolveGlob(string path)
    {
        var directory = Path.GetDirectoryName(path);

        if (string.IsNullOrEmpty(directory))
            directory = Directory.GetCurrentDirectory();

        var pattern = Path.GetFileName(path);

        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Директория не найдена: {directory}");

        var sources = Directory.GetFiles(directory, pattern)
            .Where(IsSupportedFile)
            .Select(file => new LogSource(file, SourceType.File))
            .ToArray();

        if (sources.Length == 0)
            throw new FileNotFoundException($"По маске не найдены файлы: {path}");

        return sources;
    }

    private IReadOnlyList<LogSource> ResolveFile(string path)
    {
        var fullPath = Path.GetFullPath(path);

        if (!IsSupportedFile(fullPath))
            throw new ArgumentException("Поддерживаются только файлы .log и .txt.");

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Файл не найден: {fullPath}");

        return [new LogSource(fullPath, SourceType.File)];
    }

    private bool IsUrl(string path)
        => path.StartsWith("http:", StringComparison.OrdinalIgnoreCase) || 
           path.StartsWith("https:", StringComparison.OrdinalIgnoreCase);

    private bool IsGlob(string path)
        => path.Contains('*') || path.Contains('?');

    private bool IsSupportedFile(string path)
        => _supportedExtensions.Contains(Path.GetExtension(path));
}
