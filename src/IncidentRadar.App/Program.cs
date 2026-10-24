using IncidentRadar.App.CLI;

public class Program
{
    public static int Main(string[] args)
    {
        var rootCommand = CommandFactory.CreateRoot();
        var parseResult = rootCommand.Parse(args);

        
        return 0;
    }
}