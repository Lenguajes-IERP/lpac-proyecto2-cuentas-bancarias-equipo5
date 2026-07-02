using System;
using System.Linq;
using System.Windows;

namespace SalesPro.Wpf;

public partial class App : Application
{
    private const string DarkThemePath = "Themes/DarkTheme.xaml";
    private const string LightThemePath = "Themes/LightTheme.xaml";

    public static bool IsDarkTheme { get; private set; } = true;

    public static void CambiarTema(bool usarTemaOscuro)
    {
        if (Current is null)
        {
            return;
        }

        var diccionarios = Current.Resources.MergedDictionaries;
        var temaActual = diccionarios.FirstOrDefault(diccionario =>
            diccionario.Source is not null
            && (diccionario.Source.OriginalString.EndsWith(DarkThemePath, StringComparison.OrdinalIgnoreCase)
                || diccionario.Source.OriginalString.EndsWith(LightThemePath, StringComparison.OrdinalIgnoreCase)));

        if (temaActual is not null)
        {
            diccionarios.Remove(temaActual);
        }

        var rutaTema = usarTemaOscuro ? DarkThemePath : LightThemePath;
        diccionarios.Insert(0, new ResourceDictionary
        {
            Source = new Uri(rutaTema, UriKind.Relative)
        });

        IsDarkTheme = usarTemaOscuro;
    }

    public static void AlternarTema()
    {
        CambiarTema(!IsDarkTheme);
    }
}
