using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveBinding.Engine;

public sealed class Binding<T> : IDisposable
{
    private readonly IDisposable _subscription;

    public Binding(IObservable<T> source, IObserver<T> target)
    {
        _subscription = source.Subscribe(target);
    }

    public Binding(IObserver<T> source, IObservable<T> target) : this(target, source)
    {
    }

    public Binding(ISubject<T, T> source, ISubject<T, T> target)
    {
        var bindableSource = new BindableSubject<T>(source);
        var bindableTarget = new BindableSubject<T>(target);
        
        _subscription = new CompositeDisposable(
            bindableSource.Subscribe(bindableTarget),
            bindableTarget.Subscribe(bindableSource));
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}