using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Example.WebApi;

namespace TestToDocs.Test;

public class RecordingHandler : DelegatingHandler
{
    public List<RecordedCalls> Recorded = new();
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Recorded.Add(new RecordedCalls(request.RequestUri?.AbsolutePath));
        return base.SendAsync(request, cancellationToken);
    }
}

public class TestFixture : WebApplicationFactory<Program>
{
    public List<RecordedCalls> Recorded => _recordingHandler.Recorded;
    private readonly RecordingHandler _recordingHandler = new();

    public HttpClient CreateRecordedClient() => CreateDefaultClient(_recordingHandler);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        foreach (var call in Recorded)
        {
            System.Console.WriteLine(call.Path);
        }
    }
}

public record RecordedCalls(string? Path);
