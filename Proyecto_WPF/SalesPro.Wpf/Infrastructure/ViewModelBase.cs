using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesPro.Wpf.Infrastructure;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    // Base común de los ViewModels. WPF usa PropertyChanged para refrescar la pantalla
    // cuando cambia una propiedad enlazada con Binding.
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        // Si el valor no cambió, no se notifica nada. Esto evita refrescos innecesarios.
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        // CallerMemberName permite llamar SetProperty sin escribir el nombre de la propiedad a mano.
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        // Esta línea es la que avisa al XAML: "esta propiedad cambió, vuelva a pintarla".
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
