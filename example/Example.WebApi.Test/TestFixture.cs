using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;

namespace TestToDocs.Test;

public class TestFixture : WebApplicationFactory<Program>
{
    public OpenApiDocument GenerateOpenApiDocument() => _recordingFixture.GenerateOpenApiDocument();
    private readonly RecordingFixture _recordingFixture = new(x =>
    {
        string fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../example-spec.yaml");
        File.WriteAllText(fileLocation, x.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Yaml));
    });

    public HttpClient CreateRecordedClient() => this.CreateDefaultClient(_recordingFixture.CreateHandler());

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
