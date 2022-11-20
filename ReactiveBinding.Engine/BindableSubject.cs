using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBinding.Engine;

internal sealed class BindableSubject<TSink, TEmitter> : ISubject<TSink, TEmitter>
{
    private readonly IObserver<TSink> _sink;
    private readonly IConnectableObservable<TEmitter> _emitter;

    private IDisposable _emitterConnection;

    public BindableSubject(ISubject<TSink, TEmitter> subject)
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

    public void OnNext(TSink value)
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

    public IDisposable Subscribe(IObserver<TEmitter> observer)
    {
        return _emitter.Subscribe(observer);
    }
}