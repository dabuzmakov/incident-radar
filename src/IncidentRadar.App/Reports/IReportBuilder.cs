using IncidentRadar.App.Analysis;

namespace IncidentRadar.App.Reports;

public interface IReportBuilder
{
    public string CreateReport(AnalysisResult analysisResult);
}
