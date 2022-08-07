using System.Net;

namespace TestToDocs;

public record RecordedCalls(HttpMethod HttpMethod, string? Path, HttpStatusCode? StatusCode);
