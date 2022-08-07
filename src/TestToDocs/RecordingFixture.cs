using Microsoft.OpenApi.Models;

namespace TestToDocs;

public class RecordingFixture : IDisposable
{
    private readonly List<RecordedCalls> _recorded = new();
    public IReadOnlyList<RecordedCalls> Recorded => _recorded;

    public DelegatingHandler CreateHandler() => new RecordingHandler(request =>
    {
        _recorded.Add(new RecordedCalls(request.Method, request.RequestUri?.AbsolutePath));
    });

    public void Dispose()
    {
        foreach (var call in _recorded)
        {
            System.Console.WriteLine(call.Path);
        }
    }

    public OpenApiDocument GenerateOpenApiDocument() => RecordedCallsAsOpenApiSpec.CreateOpenApi(this);
}
