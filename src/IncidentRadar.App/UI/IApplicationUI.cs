namespace IncidentRadar.App.UI;

public interface IApplicationUI
{
    public void Write(string text);
    public void WriteLine(string text);
    public bool ConfirmOverwrite(string path);
}
