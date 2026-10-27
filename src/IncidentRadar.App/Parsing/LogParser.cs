using IncidentRadar.App.Extensions;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace IncidentRadar.App.Parsing;

public static class LogParser
{
    private static readonly Regex _regex = new(
        @"^(?<ip>\S+) - (?<user>\S+) \[(?<time>[^\]]+)\] ""(?<request>[^""]*)"" (?<status>\d{3}) (?<bytes>\d+) ""(?<referer>[^""]*)"" ""(?<agent>[^""]*)""$",
        RegexOptions.Compiled);

    private static readonly Regex _protocolRegex =
        new(@"^HTTP/[0-9]+\.[0-9]+$", RegexOptions.Compiled);

    public static bool TryParse(string line, out LogEntry? entry)
    {
        var match = _regex.Match(line);
        entry = null;

        if (!match.Success) return false;

        if (!IPAddress.TryParse(match.Groups["ip"].Value, out var ipAddress))
            return false;

        var timeText = match.Groups["time"].Value;
        if (timeText.Length < 5) return false;

        timeText = timeText.Insert(timeText.Length - 2, ":");

        if (!DateTimeOffset.TryParseExact(
                timeText,
                "dd/MMM/yyyy:HH:mm:ss zzz",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var timestamp))
            return false;

        var requestText = match.Groups["request"].Value;
        var requestParts = requestText.Split(' ');

        if (requestParts.Length != 3) return false;

        if (!HttpMethod.TryParse(requestParts[0], out var method))
            return false;

        var resource = requestParts[1];
        if (string.IsNullOrWhiteSpace(resource))
            return false;

        var protocol = requestParts[2];

        if (!_protocolRegex.IsMatch(protocol))
            return false;

        var request = new HttpRequest(method, resource, protocol);

        if (!int.TryParse(match.Groups["status"].Value, out var status) ||
            status is < 100 or > 599)
            return false;

        if (!long.TryParse(match.Groups["bytes"].Value, out var bytes))
            return false;

        var user = NullIfDash(match.Groups["user"].Value);
        var referer = NullIfDash(match.Groups["referer"].Value);
        var agent = NullIfDash(match.Groups["agent"].Value);

        entry = new LogEntry(
            RemoteAddress: ipAddress,
            RemoteUser: user,
            TimeLocal: timestamp,
            Request: request,
            HttpStatus: (HttpStatusCode)status,
            ResponseBytes: bytes,
            Referer: referer,
            UserAgent: agent
        );

        return true;
    }

    private static string? NullIfDash(string value)
        => value == "-" ? null : value;
}
