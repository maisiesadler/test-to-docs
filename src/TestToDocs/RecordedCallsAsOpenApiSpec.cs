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
            doc.Paths.Add(recordedCall.Path, new OpenApiPathItem { });
        }

        return doc;
    }
}
