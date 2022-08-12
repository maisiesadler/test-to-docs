using System.Net;
using Microsoft.OpenApi.Models;

namespace TestToDocs.Test;

public class GenerateOpenApiForWebApiTest : IClassFixture<TestFixture>
{
    public TestFixture _testFixture;

    public GenerateOpenApiForWebApiTest(TestFixture testFixture)
    {
        _testFixture = testFixture;
    }

    [Fact]
    public async Task NotFound()
    {
        // Arrange
        var client = _testFixture.CreateRecordedClient();

        // Act
        var response = await client.GetAsync("/not-found");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var document = _testFixture.GenerateOpenApiDocument();
        Assert.True(document.Paths.TryGetValue("/not-found", out var pathValue));

        var (operationType, operationValue) = Assert.Single(pathValue!.Operations);
        Assert.Equal(OperationType.Get, operationType);

        var (responseStatus, responseValue) = Assert.Single(operationValue.Responses);
        Assert.Equal("404", responseStatus);

        var (contentType, contentValue) = Assert.Single(responseValue.Content);
        Assert.Equal("application/json; charset=utf-8", contentType);

        Assert.Equal("object", contentValue.Schema.Type);
        var (propertyName, propertySchema) = Assert.Single(contentValue.Schema.Properties);

        Assert.Equal("wtf", propertyName);
        Assert.Equal("string", propertySchema.Type);
    }

    [Fact]
    public async Task NestedResponseObjectsRecorded()
    {
        // Arrange
        var client = _testFixture.CreateRecordedClient();

        // Act
        var response = await client.GetAsync("/nested-response");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var document = _testFixture.GenerateOpenApiDocument();
        Assert.True(document.Paths.TryGetValue("/nested-response", out var pathValue));

        var (operationType, operationValue) = Assert.Single(pathValue!.Operations);
        Assert.Equal(OperationType.Get, operationType);

        var (responseStatus, responseValue) = Assert.Single(operationValue.Responses);
        Assert.Equal("200", responseStatus);

        var (contentType, contentValue) = Assert.Single(responseValue.Content);
        Assert.Equal("application/json; charset=utf-8", contentType);

        Assert.Equal("object", contentValue.Schema.Type);
        var (propertyName, propertySchema) = Assert.Single(contentValue.Schema.Properties);

        Assert.Equal("this", propertyName);
        Assert.Equal("object", propertySchema.Type);
        var (nestedPropertyName, nestedPropertySchema) = Assert.Single(propertySchema.Properties);

        Assert.Equal("isNested", nestedPropertyName);
        Assert.Equal("integer", nestedPropertySchema.Type);
    }
}
