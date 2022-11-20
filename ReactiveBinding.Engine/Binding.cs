using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveBinding.Engine;

public sealed class Binding : IDisposable
{
    private readonly IDisposable _subscription;

    public Binding(IObservable<string> source, IObserver<string> target)
    {
        _subscription = source.Subscribe(target);
    }

    public Binding(IObserver<string> source, IObservable<string> target)
    {
        _subscription = target.Subscribe(source);
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}