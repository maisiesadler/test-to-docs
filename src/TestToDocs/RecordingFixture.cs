using Microsoft.OpenApi.Models;

namespace TestToDocs;

public class RecordingFixture : IDisposable
{
    private readonly List<RecordedCall> _recorded = new();
    private readonly Action<OpenApiDocument>? _onDispose;

    internal IReadOnlyList<RecordedCall> Recorded => _recorded;

    public RecordingFixture(Action<OpenApiDocument>? onDispose = null)
    {
        _onDispose = onDispose;
    }

    public DelegatingHandler CreateHandler() => new RecordingHandler((request, response, exception) =>
    {
        var contentType = GetContentType(response);
        _recorded.Add(new RecordedCall(request.Method, request.RequestUri?.AbsolutePath, response?.StatusCode, contentType));
    });

    private static string? GetContentType(HttpResponseMessage? responseMessage)
    {
        if (responseMessage?.Content == null) return null;
        if (!responseMessage.Content.Headers.TryGetValues("Content-Type", out var contentHeaders)) return null;

        return string.Join("", contentHeaders);
    }

    public void Dispose()
    {
        _onDispose?.Invoke(GenerateOpenApiDocument());
    }

    public OpenApiDocument GenerateOpenApiDocument() => RecordedCallsAsOpenApiSpec.CreateOpenApi(this);
}
