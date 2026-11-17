namespace IncidentRadar.App.Extensions;

public static class HttpMethodExtensions
{
    extension(HttpMethod)
    {
        public static bool TryParse(string methodName, out HttpMethod? result)
        {
            try
            {
                result = HttpMethod.Parse(methodName);
            }
            catch (Exception exception) when (
                exception is ArgumentException
                or FormatException)
            {
                result = null;
                return false;
            }

            return true;
        }
    }
}
