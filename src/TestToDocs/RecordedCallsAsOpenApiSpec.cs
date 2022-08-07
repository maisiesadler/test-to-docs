using Microsoft.OpenApi.Models;

namespace TestToDocs;

internal class RecordedCallsAsOpenApiSpec
{
    internal static OpenApiDocument CreateOpenApi(RecordingFixture recordingFixture)
    {
        var doc = new OpenApiDocument
        {
            Paths = new OpenApiPaths(),
        };

        foreach (var recordedCall in recordingFixture.Recorded)
        {
            if (!doc.Paths.ContainsKey(recordedCall.Path))
                doc.Paths.Add(recordedCall.Path, new OpenApiPathItem { });

            var operationType = AsOperationType(recordedCall.HttpMethod);
            if (!doc.Paths[recordedCall.Path].Operations.ContainsKey(operationType))
                doc.Paths[recordedCall.Path].Operations.Add(operationType, new OpenApiOperation());
        }

        return doc;
    }

    private static OperationType AsOperationType(HttpMethod httpMethod)
    {
        return httpMethod.ToString() switch
        {
            "GET" => OperationType.Get,
            "POST" => OperationType.Post,
            _ => throw new InvalidOperationException("Unexpected HttpMethod: " + httpMethod),
        };
    }
}
