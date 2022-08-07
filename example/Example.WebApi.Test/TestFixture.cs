using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Example.WebApi;

namespace TestToDocs.Test;

public class TestFixture : WebApplicationFactory<Program>
{
    public IReadOnlyList<RecordedCalls> Recorded => _recordingFixture.Recorded;
    private readonly RecordingFixture _recordingFixture = new();
    public HttpClient CreateRecordedClient() => this.CreateRecordedClient(_recordingFixture);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _recordingFixture.Dispose();
    }
}
