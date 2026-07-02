using System.Windows.Input;

namespace SalesPro.Wpf.Infrastructure;

public sealed class AsyncRelayCommand : ICommand
{
    // Versión async del comando. Se usa cuando hay llamadas HTTP o trabajo que debe esperarse.
    private readonly Func<Task> _execute;
    private readonly Func<bool>? _canExecute;
    private bool _isRunning;

    public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        // Mientras se está ejecutando, se deshabilita para evitar doble clic y doble solicitud.
        return !_isRunning && (_canExecute?.Invoke() ?? true);
    }

    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        try
        {
            _isRunning = true;
            RaiseCanExecuteChanged();
            // Aquí se espera la tarea real del ViewModel, por ejemplo cargar o guardar.
            await _execute();
        }
        finally
        {
            // Aunque falle, el botón se vuelve a habilitar según la condición original.
            _isRunning = false;
            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
