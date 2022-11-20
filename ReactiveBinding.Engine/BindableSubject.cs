using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBinding.Engine;

internal sealed class BindableSubject : ISubject<string, string>
{
    private readonly IObserver<string> _sink;
    private readonly IConnectableObservable<string> _emitter;

    private IDisposable _emitterConnection;

    public BindableSubject(ISubject<string, string> subject)
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

    public void OnNext(string value)
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

    public IDisposable Subscribe(IObserver<string> observer)
    {
        return _emitter.Subscribe(observer);
    }
}