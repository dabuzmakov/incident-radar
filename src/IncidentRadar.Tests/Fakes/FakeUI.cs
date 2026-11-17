using IncidentRadar.App.UI;

namespace IncidentRadar.Tests.Fakes;

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
