using System.Reactive.Subjects;
using NSubstitute;

namespace ReactiveBinding.Engine.Tests;

public class EngineTests
{
    [Fact]
    public void Target_receive_values_from_source()
    {
        var source = new Subject<string>();
        var target = Substitute.For<IObserver<string>>();

        _ = new Binding(source, target);
        source.OnNext("test");
        
        target.Received().OnNext("test");
    }

    [Fact]
    public void Binding_stop_working_when_disposed()
    {
        var source = new Subject<string>();
        var target = Substitute.For<IObserver<string>>();

        var binding = new Binding(source, target);
        binding.Dispose();
        source.OnNext("test");
        
        target.DidNotReceive().OnNext(Arg.Any<string>());
    }

    [Fact]
    public void Source_receives_updates_from_target()
    {
        var source = Substitute.For<IObserver<string>>();
        var target = new Subject<string>();

        _ = new Binding(source, target);
        target.OnNext("test");
        
        source.Received().OnNext("test");
    }

    [Fact]
    public void Binging_is_bidirectional()
    {
        var source = new Subject<string>();
        var sourceListener = Substitute.For<IObserver<string>>();
        source.Subscribe(sourceListener);
        
        var target = new Subject<string>();
        var targetListener = Substitute.For<IObserver<string>>();
        target.Subscribe(targetListener);
        
        _ = new Binding(source, target);
        source.OnNext("source");
        target.OnNext("target");

        sourceListener.Received().OnNext("target");
        targetListener.Received().OnNext("source");
    }
}