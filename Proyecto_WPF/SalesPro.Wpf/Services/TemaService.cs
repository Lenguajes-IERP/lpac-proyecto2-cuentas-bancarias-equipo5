using System.Windows;

namespace SalesPro.Wpf.Services;

public enum Tema
{
    Claro,
    AltoContraste
}

/// <summary>
/// Intercambia en runtime el ResourceDictionary de tema en Application.Resources.
/// El tema por defecto es Claro. Traduce a WPF el sistema de temas accesibles
/// (claro / alto contraste) del proyecto InclusaSalud.
/// </summary>
public static class TemaService
{
    private const int IndiceTema = 0;

    private static readonly ResourceDictionary Claro = new()
    {
        Source = new Uri("Themes/TemaClaro.xaml", UriKind.Relative)
    };

    private static readonly ResourceDictionary AltoContraste = new()
    {
        Source = new Uri("Themes/TemaAltoContraste.xaml", UriKind.Relative)
    };

    public static Tema Actual { get; private set; } = Tema.Claro;

    public static void Aplicar(Tema tema)
    {
        var dict = tema == Tema.AltoContraste ? AltoContraste : Claro;

        var merged = Application.Current.Resources.MergedDictionaries;
        if (merged.Count > IndiceTema)
        {
            merged[IndiceTema] = dict;
        }
        else
        {
            merged.Insert(IndiceTema, dict);
        }

        Actual = tema;
    }
}
