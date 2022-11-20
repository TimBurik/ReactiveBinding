using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBinding.Engine;

internal sealed class BindableSubject<T> : ISubject<T, T>
{
    private readonly IObserver<T> _sink;
    private readonly IConnectableObservable<T> _emitter;

    private IDisposable _emitterConnection;

    public BindableSubject(ISubject<T, T> subject)
    {
        _sink = subject.AsObserver();
        _emitter = subject.Publish();

        _emitterConnection = _emitter.Connect();
    }

    public void OnCompleted()
    {
        _sink.OnCompleted();
    }

    public void OnError(Exception error)
    {
        _sink.OnError(error);
    }

    public void OnNext(T value)
    {
        _emitterConnection.Dispose();
        
        try
        {
            _sink.OnNext(value);
        }
        finally
        {
            _emitterConnection = _emitter.Connect();
        }
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _emitter.Subscribe(observer);
    }
}