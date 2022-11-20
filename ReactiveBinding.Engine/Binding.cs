using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBinding.Engine;

public sealed class Binding : IDisposable
{
    private readonly IDisposable _subscription;

    public Binding(IObservable<string> source, IObserver<string> target)
    {
        _subscription = source.Subscribe(target);
    }

    public Binding(IObserver<string> source, IObservable<string> target) : this(target, source)
    {
    }

    public Binding(ISubject<string, string> source, ISubject<string, string> target)
    {
        var bindableSource = new BindableSubject(source);
        var bindableTarget = new BindableSubject(target);
        
        _subscription = new CompositeDisposable(
            bindableSource.Subscribe(bindableTarget),
            bindableTarget.Subscribe(bindableSource));
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}