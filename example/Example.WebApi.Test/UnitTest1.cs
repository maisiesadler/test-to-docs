using System.Net;

namespace TestToDocs.Test;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        // Arrange
        using var fixture = new TestFixture();
        var client = fixture.CreateRecordedClient();

        // Act
        var response = await client.GetAsync($"/pokemon");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var recorded = Assert.Single(fixture.Recorded);
        Assert.Equal("/pokemon", recorded.Path);
    }

    [Fact]
    public async Task Test2()
    {
        // Arrange
        using var fixture = new TestFixture();
        var client = fixture.CreateRecordedClient();

        // Act
        var response = await client.GetAsync($"/pokemon-thing");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var recorded = Assert.Single(fixture.Recorded);
        Assert.Equal("/pokemon-thing", recorded.Path);
    }
}
