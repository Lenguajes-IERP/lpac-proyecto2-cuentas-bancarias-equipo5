using System.Windows.Controls;
using System.Windows.Input;

namespace SalesPro.Wpf.Views;

public partial class CuentasBancariasView : UserControl
{
    public CuentasBancariasView()
    {
        InitializeComponent();
    }

    // Accesibilidad: abrir el ComboBox con Enter o Espacio (además del F4/Alt+Abajo nativo).
    private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is ComboBox combo && (e.Key == Key.Enter || e.Key == Key.Space))
        {
            combo.IsDropDownOpen = !combo.IsDropDownOpen;
            e.Handled = true;
        }
    }
}
