using System.Windows.Input;

namespace SalesPro.Wpf.Infrastructure;

public sealed class RelayCommand : ICommand
{
    // Comando para acciones rápidas/sin await, por ejemplo limpiar formulario o incrementar cantidad.
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        // Si no se definió condición, el comando siempre está habilitado.
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        // WPF llama Execute cuando el usuario presiona el botón asociado.
        _execute();
    }

    public void RaiseCanExecuteChanged()
    {
        // Avisa a WPF que vuelva a preguntar si el botón debe estar habilitado o deshabilitado.
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
