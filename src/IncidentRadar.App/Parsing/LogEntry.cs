using System.Net;

namespace IncidentRadar.App.Parsing;

public record LogEntry(
    IPAddress RemoteAddress,
    string? RemoteUser,
    DateTimeOffset TimeLocal,
    HttpRequest Request,
    HttpStatusCode HttpStatus,
    long ResponseSize,
    string? Referer,
    string? UserAgent)
{
    public bool InBounds(DateOnly? fromDate, DateOnly? toDate)
    {
        var logDate = DateOnly.FromDateTime(TimeLocal.DateTime);

        if (fromDate is { } from && logDate < from)
        {
            return false;
        }

        if (toDate is { } to && logDate > to)
        {
            return false;
        }

        return true;
    }
}