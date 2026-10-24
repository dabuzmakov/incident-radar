using IncidentRadar.App.Analysis;

namespace IncidentRadar.App.CLI;

public record ParseResult(
    AnalysisOptions? Options, 
    IReadOnlyList<string> Errors)
{
    public bool IsSuccess => Errors.Count == 0;
}
