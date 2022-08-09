namespace TestToDocs.Test;

public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage?> TrySendAsync(this HttpClient httpClient, HttpMethod httpMethod, string path)
        => httpClient.TrySendAsync(httpMethod, new Uri("https://example.local" + path));

    public static async Task<HttpResponseMessage?> TrySendAsync(this HttpClient httpClient, HttpMethod httpMethod, Uri uri)
    {
        var request = new HttpRequestMessage
        {
            RequestUri = uri,
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
