namespace TestToDocs;

internal class RecordingHandler : DelegatingHandler
{
    private readonly Action<HttpRequestMessage> _onSend;

    public RecordingHandler(Action<HttpRequestMessage> onSend)
    {
        _onSend = onSend;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _onSend(request);
        return base.SendAsync(request, cancellationToken);
    }
}
