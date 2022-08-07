using System.Net;

namespace TestToDocs.Test;

public class GenerateOpenApiForWebApiTest : IClassFixture<TestFixture>
{
    public TestFixture _testFixture;

    public GenerateOpenApiForWebApiTest(TestFixture testFixture)
    {
        _testFixture = testFixture;
    }

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var client = _testFixture.CreateRecordedClient();

        // Act
        var response = await client.GetAsync($"/pokemon");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var recorded = Assert.Single(_testFixture.Recorded.Where(r => r.Path == "/pokemon"));
        Assert.Equal(HttpStatusCode.NotFound, recorded.StatusCode);
    }

    [Fact]
    public async Task Test2()
    {
        // Arrange
        var client = _testFixture.CreateRecordedClient();

        // Act
        var response = await client.GetAsync($"/pokemon-thing");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var recorded = Assert.Single(_testFixture.Recorded.Where(r => r.Path == "/pokemon-thing"));
        Assert.Equal(HttpStatusCode.NotFound, recorded.StatusCode);
    }
}
