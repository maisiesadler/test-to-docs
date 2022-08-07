namespace TestToDocs;

internal class RecordingHandler : DelegatingHandler
{
    private readonly Action<HttpRequestMessage, HttpResponseMessage?, Exception?> _onSend;

    public RecordingHandler(Action<HttpRequestMessage, HttpResponseMessage?, Exception?> onSend)
    {
        _onSend = onSend;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            _onSend(request, response, null);
            return response;
        }
        catch (Exception ex)
        {
            _onSend(request, null, ex);
            throw;
        }
    }
}
