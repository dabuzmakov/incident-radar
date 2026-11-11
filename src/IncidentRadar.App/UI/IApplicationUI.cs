namespace IncidentRadar.App.UI;

public interface IApplicationUI
{
    public void WriteLine(string text);

    public bool ConfirmOverwrite();
}
