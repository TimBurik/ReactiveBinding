using NSubstitute;
using ReactiveBinding.Engine;
using ReactiveBinding.Extensions.Tests.Utils;

namespace ReactiveBinding.Extensions.Tests;

public class ObservableObjectTests
{
    [Fact]
    public void Observable_object_changes_are_passed_to_target()
    {
        var source = new TestObservableObject<string>(string.Empty);
        var target = Substitute.For<ITestValueReceiver<string>>();

        _ = BindingEngine.Bind(
            source.AsEmitter(nameof(source.Property), x => x.Property),
            target.AsReceiver<ITestValueReceiver<string>, string>((x, value) => x.Property = value));
        source.Property = "test";

        target.Received().Property = "test";
    }
}