using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfBuoyancy.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Helper générique pour setter une propriété + notification centralisée.
    /// </summary>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        bool changed = false;
        if (!Equals(field, value))
        {
            field = value;
            changed = true;
            innerOnPropertyChanged(propertyName);
            OnPropertyChanged(propertyName);
        }
        return changed;
    }
    protected virtual void innerOnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
    }
}