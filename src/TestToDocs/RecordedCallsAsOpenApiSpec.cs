using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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
        if (TryGetContentType(recordedCall, out var openApiMediaType))
        {
            response.Content[recordedCall.ResponseContentType] = openApiMediaType;
        }
        operation.Responses.Add(((int?)recordedCall.StatusCode).ToString(), response);
    }

    private static bool TryGetContentType(RecordedCall recordedCall, [NotNullWhen(true)] out OpenApiMediaType? openApiMediaType)
    {
        if (recordedCall.ResponseContentType == null || recordedCall.Content == null)
        {
            openApiMediaType = null;
            return false;
        }

        if (recordedCall.ResponseContentType.StartsWith("application/json"))
        {
            var content = JsonDocument.Parse(recordedCall.Content);
            if (content == null)
            {
                openApiMediaType = null;
                return false;
            }

            openApiMediaType = new OpenApiMediaType
            {
                Schema = GetOpenApiSchema(content.RootElement),
            };

            return true;
        }

        if (recordedCall.ResponseContentType.StartsWith("text/html"))
        {
            openApiMediaType = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Properties = new Dictionary<string, OpenApiSchema>(),
                    Type = "string",
                },
            };
            return true;
        }

        openApiMediaType = null;
        return false;
    }

    private static OpenApiSchema GetOpenApiSchema(JsonElement jsonElement)
    {
        var schema = new OpenApiSchema
        {
            Properties = new Dictionary<string, OpenApiSchema>(),
            Type = GetType(jsonElement),
        };

        if (jsonElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                schema.Properties[property.Name] = GetOpenApiSchema(property.Value);
            }
        }

        return schema;
    }

    private static string GetType(JsonElement jsonElement)
    {
        return jsonElement.ValueKind switch
        {
            JsonValueKind.String => "string",
            JsonValueKind.Object => "object",
            JsonValueKind.Number => "number",
            _ => throw new InvalidOperationException($"Idk about {jsonElement.ValueKind}"),
        };
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
