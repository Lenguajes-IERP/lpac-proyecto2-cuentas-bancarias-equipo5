using System.Windows;
using SalesPro.Wpf.Services;

namespace SalesPro.Wpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        // Tema por defecto: Claro (el usuario puede cambiar a Alto Contraste en la barra).
        TemaService.Aplicar(Tema.Claro);
    }
}
