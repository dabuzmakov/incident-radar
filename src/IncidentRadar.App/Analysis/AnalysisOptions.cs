namespace IncidentRadar.App.Analysis;

public record AnalysisOptions(
    string Path,
    string OutputFormat,
    string? OutputPath,
    DateOnly? From,
    DateOnly? To);
