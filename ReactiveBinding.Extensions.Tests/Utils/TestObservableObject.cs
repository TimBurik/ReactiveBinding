using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReactiveBinding.Extensions.Tests.Utils;

public sealed class TestObservableObject<TProperty> : INotifyPropertyChanged
{
    private TProperty _propertyValue;

    public TestObservableObject(TProperty propertyValue)
    {
        _propertyValue = propertyValue;
    }

    public TProperty Property
    {
        get => _propertyValue;
        set => SetField(ref _propertyValue, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }
        
        field = value;
        OnPropertyChanged(propertyName);
    }
}