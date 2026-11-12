namespace IncidentRadar.App.UI;

public class FakeUI : IApplicationUI
{
    public bool ConfirmOverwrite(string path)
    {
        return true;
    }

    public void Write(string text)
    {
        return;
    }

    public void WriteLine(string text)
    {
        return;
    }
}
