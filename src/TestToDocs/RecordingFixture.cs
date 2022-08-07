using Microsoft.OpenApi.Models;

namespace TestToDocs;

public class RecordingFixture : IDisposable
{
    private readonly List<RecordedCalls> _recorded = new();
    private readonly Action<OpenApiDocument>? _onDispose;

    public IReadOnlyList<RecordedCalls> Recorded => _recorded;

    public RecordingFixture(Action<OpenApiDocument>? onDispose = null)
    {
        _onDispose = onDispose;
    }

    public DelegatingHandler CreateHandler() => new RecordingHandler(request =>
    {
        _recorded.Add(new RecordedCalls(request.Method, request.RequestUri?.AbsolutePath));
    });

    public void Dispose()
    {
        _onDispose?.Invoke(GenerateOpenApiDocument());
    }

    public OpenApiDocument GenerateOpenApiDocument() => RecordedCallsAsOpenApiSpec.CreateOpenApi(this);
}
