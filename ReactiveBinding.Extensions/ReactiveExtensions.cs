using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ReactiveBinding.Extensions;

public static class ReactiveExtensions
{
    public static IObservable<TResult> AsEmitter<TSource, TResult>(
        this TSource source,
        string propertyName,
        Func<TSource,  TResult> selector) where TSource : INotifyPropertyChanged
    {
        return source
            .AsPropertyChangedObservable()
            .Where(x => propertyName.Equals(x.PropertyName))
            .Select(_ => selector.Invoke(source));
    }

    public static IObserver<TResult> AsReceiver<TTarget, TResult>(
        this TTarget target,
        Action<TTarget, TResult> applier)
    {
        return Observer.Create<TResult>(x => applier.Invoke(target, x));
    }

    public static IObservable<PropertyChangedEventArgs> AsPropertyChangedObservable(this INotifyPropertyChanged source)
    {
        return Observable.Create<PropertyChangedEventArgs>(ob =>
        {
            void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                ob.OnNext(e);
            }

            source.PropertyChanged += OnPropertyChanged;

            return Disposable.Create(source, x => x.PropertyChanged -= OnPropertyChanged);
        });
    }
}