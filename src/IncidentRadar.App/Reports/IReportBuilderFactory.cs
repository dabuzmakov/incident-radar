namespace IncidentRadar.App.Reports;

public interface IReportBuilderFactory
{
    public IReportBuilder GetReportBuilder(string outputFormat);
}
