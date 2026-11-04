using IncidentRadar.App.Parsing;

namespace IncidentRadar.App.Analysis.Statistics;

public interface IStatisticsCollector
{
    void Add(LogEntry entry);
    AnalysisStatistics GetStatistics();
}
