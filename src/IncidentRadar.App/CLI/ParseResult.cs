using IncidentRadar.App.Analysis;

namespace IncidentRadar.App.CLI;

public class ParseResult
{
    public bool Successed { get; private set; }
    public List<string> Errors { get; init; }

    public AnalysisOptions ToAnalysisOptions()
    {
        throw new NotImplementedException();
    }
}
