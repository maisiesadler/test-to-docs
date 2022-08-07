namespace TestToDocs.Test;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage?> TrySendAsync(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string path)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("https://example.local" + path),
            Method = httpMethod,
        };
        return await httpClient.TrySendAsync(request);
    }

    public static async Task<HttpResponseMessage?> TrySendAsync(this HttpClient httpClient, HttpRequestMessage httpRequestMessage)
    {
        try
        {
            return await httpClient.SendAsync(httpRequestMessage);
        }
        catch
        {
            return null;
        }
    }
}
