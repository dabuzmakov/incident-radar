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
        entry = null;
        var match = _regex.Match(line);

        if (!match.Success) return false;

        if (!IPAddress.TryParse(match.Groups["ip"].Value, out var ipAddress))
            return false;

        if (!TryParseTime(match.Groups["time"].Value, out var timestamp))
            return false;

        if (!TryParseRequest(match.Groups["request"].Value, out var request))
            return false;

        if (!int.TryParse(match.Groups["status"].Value, out var status) 
            || status is < 100 or > 599)
            return false;

        if (!long.TryParse(match.Groups["bytes"].Value, out var bytes))
            return false;

        entry = new LogEntry(
            RemoteAddress: ipAddress,
            RemoteUser: NullIfDash(match.Groups["user"].Value),
            TimeLocal: timestamp,
            Request: request,
            HttpStatus: (HttpStatusCode)status,
            ResponseBytes: bytes,
            Referer: NullIfDash(match.Groups["referer"].Value),
            UserAgent: NullIfDash(match.Groups["agent"].Value)
        );

        return true;
    }

    private static bool TryParseTime(string value, out DateTimeOffset timestamp)
    {
        timestamp = default;

        if (value.Length < 5)
            return false;

        var normalized = value.Insert(value.Length - 2, ":");

        return DateTimeOffset.TryParseExact(
            normalized,
            "dd/MMM/yyyy:HH:mm:ss zzz",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out timestamp);
    }

    private static bool TryParseRequest(string value, out HttpRequest request)
    {
        request = null!;
        var parts = value.Split(' ');

        if (parts.Length != 3)
            return false;

        if (!HttpMethod.TryParse(parts[0], out var method))
            return false;

        var resource = parts[1];

        if (string.IsNullOrWhiteSpace(resource))
            return false;

        var protocol = parts[2];

        if (!_protocolRegex.IsMatch(protocol))
            return false;

        request = new HttpRequest(method!, resource, protocol);

        return true;
    }

    private static string? NullIfDash(string value)
        => value == "-" ? null : value;
}
