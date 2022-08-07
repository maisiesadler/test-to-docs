namespace TestToDocs.Test;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
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
        var recorded = Assert.Single(recordingFixture.Recorded);
        Assert.Equal("/", recorded.Path);
    }
}
