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
        await client.TrySendAsync(HttpMethod.Get, "/some-resource");

        // Assert
        var openApiSpec = recordingFixture.GenerateOpenApiDocument();
        var path = Assert.Single(openApiSpec.Paths);
    }

    [Fact]
    public async Task OpenApiPathHasCorrectOperations()
    {
        // Arrange
        var recordingFixture = new RecordingFixture();
        var handler = recordingFixture.CreateHandler();
        var services = new ServiceCollection();
        services.AddHttpClient("test").AddHttpMessageHandler(_ => handler);
        var sp = services.BuildServiceProvider();
        var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("test");

        // Act
        await client.TrySendAsync(HttpMethod.Get, "/some-resource");
        await client.TrySendAsync(HttpMethod.Post, "/some-resource");

        // Assert
        var openApiSpec = recordingFixture.GenerateOpenApiDocument();
        var path = Assert.Single(openApiSpec.Paths);
    }
}
