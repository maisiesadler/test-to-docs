using Microsoft.OpenApi.Interfaces;
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
            if (recordedCall.Path != null)
            {
                var pathItem = GetOrAdd(doc.Paths, recordedCall.Path);
                TryRecordPath(pathItem, recordedCall);
            }
        }

        return doc;
    }

    private static void TryRecordPath(OpenApiPathItem pathItem, RecordedCall recordedCall)
    {
        var operationType = AsOperationType(recordedCall.HttpMethod);
        var operation = GetOrAdd(pathItem.Operations, operationType);

        var response = new OpenApiResponse();
        if (recordedCall.ResponseContentType != null)
        {
            response.Content[recordedCall.ResponseContentType] = new OpenApiMediaType();
        }
        operation.Responses.Add(((int?)recordedCall.StatusCode).ToString(), response);
    }

    private static TValue GetOrAdd<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key)
        where TValue : IOpenApiSerializable, new()
    {
        if (!dict.ContainsKey(key))
            dict[key] = new();

        return dict[key];
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
