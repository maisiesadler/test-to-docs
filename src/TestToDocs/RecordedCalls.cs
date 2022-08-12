using System.Net;

namespace TestToDocs;

public record RecordedCall(HttpMethod HttpMethod, string? Path, HttpStatusCode? StatusCode, string? ResponseContentType, string? Content);
