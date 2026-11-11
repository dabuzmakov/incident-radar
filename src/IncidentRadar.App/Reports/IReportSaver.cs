namespace IncidentRadar.App.Reports;

public interface IReportSaver
{
    public void Save(string report, string path, bool overwrite);
    public string PreparePath(string path);
}
