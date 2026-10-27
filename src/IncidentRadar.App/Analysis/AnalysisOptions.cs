namespace IncidentRadar.App.Analysis;

public record AnalysisOptions(
    string Path,
    string OutputFormat,
    string? OutputPath = null,
    DateOnly? From = null,
    DateOnly? To = null);
