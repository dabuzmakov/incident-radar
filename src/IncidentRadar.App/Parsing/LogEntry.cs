using System.Net;

namespace IncidentRadar.App.Parsing;

public record LogEntry(
    IPAddress RemoteAddress,
    string? RemoteUser,
    DateTimeOffset TimeLocal,
    HttpRequest Request,
    HttpStatusCode httpStatus,
    long responseBytes,
    string? Referer,
    string? UserAgent);