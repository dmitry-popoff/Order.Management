using System.Windows;
using System.Windows.Controls;

namespace Orders.Management.UI.Views;

/// <summary>
/// Interaction logic for CreateCounterpartyView.xaml
/// </summary>
public partial class CreateCounterpartyView : UserControl
{
    public CreateCounterpartyView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Window parentWindow = Window.GetWindow(this);

        parentWindow?.Close();
    }   
}
