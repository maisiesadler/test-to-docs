namespace TestToDocs;

public class RecordingFixture
{
    private readonly List<RecordedCalls> _recorded = new();
    public IReadOnlyList<RecordedCalls> Recorded => _recorded;

    internal DelegatingHandler CreateHandler() => new RecordingHandler(request =>
    {
        _recorded.Add(new RecordedCalls(request.RequestUri?.AbsolutePath));
    });
}
