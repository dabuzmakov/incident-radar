using IncidentRadar.App.Sources;

namespace IncidentRadar.App.Reports;

public class ReportBuilderFactory : IReportBuilderFactory
{
    private IReportBuilder _jsonReportBuilder;
    private IReportBuilder _markdownReportBuilder;

    public ReportBuilderFactory(
        JsonReportBuilder jsonReportBuilder,
        MarkdownReportBuilder markdownReportBuilder)
    {
        _jsonReportBuilder = jsonReportBuilder;
        _markdownReportBuilder = markdownReportBuilder;
    }

    public IReportBuilder GetReportBuilder(string outputFormat)
    {
        return outputFormat switch
        {
            "json" => _jsonReportBuilder,
            "markdown" => _markdownReportBuilder,
            _ => throw new ArgumentOutOfRangeException(outputFormat)
        };
    }
}
