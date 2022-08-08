using Microsoft.OpenApi.Models;

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
        Assert.Equal("/some-resource", path.Key);
        var operation = Assert.Single(path.Value.Operations);
        Assert.Equal(OperationType.Get, operation.Key);
        Assert.NotNull(operation.Value);
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
        Assert.Equal("/some-resource", path.Key);
        Assert.Equal(2, path.Value.Operations.Count());
        var hasGet = path.Value.Operations.TryGetValue(OperationType.Get, out var getOperation);
        Assert.True(hasGet);
        Assert.NotNull(getOperation);
        var hasPost = path.Value.Operations.TryGetValue(OperationType.Get, out var postOperation);
        Assert.True(hasPost);
        Assert.NotNull(postOperation);
    }

    [Fact]
    public async Task StatusCodeRecorded()
    {
        // Arrange
        var recordingFixture = new RecordingFixture();
        var handler = recordingFixture.CreateHandler();
        var services = new ServiceCollection();
        services.AddHttpClient("test").AddHttpMessageHandler(_ => handler);
        var sp = services.BuildServiceProvider();
        var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("test");

        // Act
        await client.TrySendAsync(HttpMethod.Get, new Uri("https://example.com/some-resource"));

        // Assert
        var openApiSpec = recordingFixture.GenerateOpenApiDocument();
        var path = Assert.Single(openApiSpec.Paths);
        Assert.Equal("/some-resource", path.Key);
        var operation = Assert.Single(path.Value.Operations);
        Assert.Equal(OperationType.Get, operation.Key);
        var response = Assert.Single(operation.Value.Responses);
        Assert.Equal("404", response.Key);
        var content = Assert.Single(response.Value.Content);
        Assert.Equal("text/html; charset=utf-8", content.Key);
    }
}
