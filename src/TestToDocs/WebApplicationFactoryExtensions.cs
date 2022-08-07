using Microsoft.AspNetCore.Mvc.Testing;

namespace TestToDocs;

public static class WebApplicationFactoryExtensions
{
    public static HttpClient CreateRecordedClient<T>(
        this WebApplicationFactory<T> webApplicationFactory,
        RecordingFixture recordingFixture)
        where T : class
    {
        var recordingHandler = recordingFixture.CreateHandler();
        return webApplicationFactory.CreateDefaultClient(recordingHandler);
    }
}
