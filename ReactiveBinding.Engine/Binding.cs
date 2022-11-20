namespace ReactiveBinding.Engine;

public sealed class Binding : IDisposable
{
    private readonly IDisposable _subscription;

    public Binding(IObservable<string> source, IObserver<string> target)
    {
        _subscription = source.Subscribe(target);
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}