using IncidentRadar.App.Exceptions;

namespace IncidentRadar.App.Reports;

public class FileReportSaver : IReportSaver
{
    public void Save(string report, string path, bool overwrite)
    {
        var mode = overwrite ? FileMode.Create : FileMode.CreateNew;

        using var stream = new FileStream(path, mode, FileAccess.Write);
        using var writer = new StreamWriter(stream);

        writer.Write(report);
    }

    public string PreparePath(string outputPath)
    {
        try
        {
            var fullPath = Path.GetFullPath(outputPath);

            if (Directory.Exists(fullPath))
            {
                throw new OutputException(
                    $"Путь отчёта указывает на директорию: {fullPath}");
            }

            var directory = Path.GetDirectoryName(fullPath);

            if (directory is null || !Directory.Exists(directory))
            {
                throw new OutputException(
                    $"Директория для отчёта не найдена: {directory}");
            }

            return fullPath;
        }
        catch (OutputException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new OutputException(
                $"Не удалось подготовить путь отчёта: {outputPath}",
                exception);
        }
    }
}
