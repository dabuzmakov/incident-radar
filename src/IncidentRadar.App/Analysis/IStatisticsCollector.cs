using IncidentRadar.App.Parsing;

namespace IncidentRadar.App.Analysis;

public interface IStatisticsCollector
{
    void Add(LogEntry entry);
    AnalysisStatistics GetStatistics();
}
