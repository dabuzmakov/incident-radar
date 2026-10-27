using System.Net;

namespace IncidentRadar.App.Parsing;

public record LogEntry(
    IPAddress RemoteAddress,
    string? RemoteUser,
    DateTimeOffset TimeLocal,
    HttpRequest Request,
    HttpStatusCode HttpStatus,
    long ResponseBytes,
    string? Referer,
    string? UserAgent);