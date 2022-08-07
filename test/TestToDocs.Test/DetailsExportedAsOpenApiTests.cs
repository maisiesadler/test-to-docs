namespace TestToDocs.Test;

public class DetailsExportedAsOpenApiTests
{
    [Fact]
    public async Task OpenApiSpecContainsPath()
    {
        // Arrange
        var recordingFixture = new RecordingFixture();
        var handler = recordingFixture.CreateHandler();
        var services = new ServiceCollection();
        services.AddHttpClient("test").AddHttpMessageHandler(_ => handler);
        var sp = services.BuildServiceProvider();
        var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("test");

        // Act
        var response = await client.GetAsync("https://example.com");

        // Assert
        var openApiSpec = recordingFixture.GenerateOpenApiDocument();
        var path = Assert.Single(openApiSpec.Paths);
    }
}
