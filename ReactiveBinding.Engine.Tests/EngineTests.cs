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
}