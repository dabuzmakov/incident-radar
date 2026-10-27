namespace IncidentRadar.App.Parsing;

public record HttpRequest(
    HttpMethod Method,
    string Resource,
    Version HttpVersion);

