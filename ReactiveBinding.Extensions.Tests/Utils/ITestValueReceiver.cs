namespace ReactiveBinding.Extensions.Tests.Utils;

public interface ITestValueReceiver<T>
{
    T Property { get; set; }
}