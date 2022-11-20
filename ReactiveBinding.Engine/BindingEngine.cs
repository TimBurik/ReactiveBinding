using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveBinding.Engine;

public static class BindingEngine
{
    public static IDisposable Bind<T>(IObservable<T> source, IObserver<T> target)
    {
        return source.Subscribe(target);
    }

    public static IDisposable Bind<T>(IObserver<T> source, IObservable<T> target)
    {
        return Bind(target, source);
    }

    public static IDisposable Bind<TSource, TTarget>(ISubject<TSource, TTarget> source, ISubject<TTarget, TSource> target)
    {
        var bindableSource = new BindableSubject<TSource, TTarget>(source);
        var bindableTarget = new BindableSubject<TTarget, TSource>(target);
        
        return new CompositeDisposable(
            bindableSource.Subscribe(bindableTarget),
            bindableTarget.Subscribe(bindableSource));
    }
}