using System.Reactive.Subjects;
using NSubstitute;
using NSubstitute.Extensions;

namespace ReactiveBinding.Engine.Tests;

public class EngineTests
{
    [Fact]
    public void Binding_passes_values_from_source_to_target()
    {
        var source = new Subject<string>();
        var target = Substitute.For<IObserver<string>>();

        _ = BindingEngine.Bind(source, target);
        source.OnNext("test");
        
        target.Received().OnNext("test");
    }
    
    [Fact]
    public void Binding_works_with_value_types()
    {
        var source = new Subject<int>();
        var target = Substitute.For<IObserver<int>>();

        _ = BindingEngine.Bind(source, target);
        source.OnNext(42);
        
        target.Received().OnNext(42);
    }

    [Fact]
    public void Disposed_binding_does_not_pass_values_from_source_to_target()
    {
        var source = new Subject<string>();
        var target = Substitute.For<IObserver<string>>();

        var binding = BindingEngine.Bind(source, target);
        binding.Dispose();
        source.OnNext("test");
        
        target.DidNotReceive().OnNext(Arg.Any<string>());
    }

    [Fact]
    public void Binding_passes_values_from_target_to_source()
    {
        var source = Substitute.For<IObserver<string>>();
        var target = new Subject<string>();

        _ = BindingEngine.Bind(source, target);
        target.OnNext("test");
        
        source.Received().OnNext("test");
    }
    
    [Fact]
    public void Disposed_binding_does_not_pass_values_from_target_to_source()
    {
        var source = Substitute.For<IObserver<string>>();
        var target = new Subject<string>();

        var binding = BindingEngine.Bind(source, target);
        binding.Dispose();
        target.OnNext("test");
        
        source.DidNotReceive().OnNext(Arg.Any<string>());
    }

    [Fact]
    public void Bidirectional_binding_passes_values_from_source_to_target_and_from_target_to_source()
    {
        var source = new Subject<string>();
        var sourceListener = Substitute.For<IObserver<string>>();
        source.Subscribe(sourceListener);
        
        var target = new Subject<string>();
        var targetListener = Substitute.For<IObserver<string>>();
        target.Subscribe(targetListener);
        
        _ = BindingEngine.Bind(source, target);
        source.OnNext("source");
        target.OnNext("target");

        sourceListener.Received().OnNext("target");
        targetListener.Received().OnNext("source");
    }

    [Fact]
    public void Disposed_bidirectional_binding_does_not_pass_values_from_source_to_target()
    {
        var source = new Subject<string>();

        var target = new Subject<string>();
        var targetListener = Substitute.For<IObserver<string>>();
        target.Subscribe(targetListener);
        
        var binding = BindingEngine.Bind(source, target);
        binding.Dispose();
        source.OnNext("source");
        
        targetListener.DidNotReceive().OnNext(Arg.Any<string>());
    }
    
    [Fact]
    public void Disposed_bidirectional_binding_does_not_pass_values_from_target_to_source()
    {
        var source = new Subject<string>();
        var sourceListener = Substitute.For<IObserver<string>>();
        source.Subscribe(sourceListener);

        var target = new Subject<string>();

        var binding = BindingEngine.Bind(source, target);
        binding.Dispose();
        target.OnNext("target");
        
        sourceListener.DidNotReceive().OnNext(Arg.Any<string>());
    }
    
    [Fact]
    public void Disposed_bidirectional_binding_does_not_pass_values_both_ways()
    {
        var source = new Subject<string>();
        var sourceListener = Substitute.For<IObserver<string>>();
        source.Subscribe(sourceListener);
        
        var target = new Subject<string>();
        var targetListener = Substitute.For<IObserver<string>>();
        target.Subscribe(targetListener);
        
        var binding = BindingEngine.Bind(source, target);
        binding.Dispose();
        source.OnNext("source");
        target.OnNext("target");

        sourceListener.DidNotReceive().OnNext("target");
        targetListener.DidNotReceive().OnNext("source");
    }

    [Fact]
    public void Bidirectional_bindings_support_multi_type_subjects()
    {
        var source = Substitute.For<ISubject<string, int>>();
        var sourceEmitter = new Subject<int>();
        source.Configure()
            .Subscribe(Arg.Any<IObserver<int>>()).Returns(x => sourceEmitter.Subscribe(x.Arg<IObserver<int>>()));

        var target = Substitute.For<ISubject<int, string>>();
        var targetEmitter = new Subject<string>();
        target.Configure()
            .Subscribe(Arg.Any<IObserver<string>>()).Returns(x => targetEmitter.Subscribe(x.Arg<IObserver<string>>()));

        _ = BindingEngine.Bind(source, target);
        sourceEmitter.OnNext(42);
        targetEmitter.OnNext("test");
        
        target.Received().OnNext(42);
        source.Received().OnNext("test");
    }
}