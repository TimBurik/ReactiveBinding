namespace ReactiveBinding.Engine;

public class Binding
{
    public Binding(IObservable<string> source, IObserver<string> target)
    {
        source.Subscribe(target);
    }
}