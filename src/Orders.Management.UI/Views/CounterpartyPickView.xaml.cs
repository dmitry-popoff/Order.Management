using System.Windows;
using System.Windows.Controls;

namespace Orders.Management.UI.Views;

/// <summary>
/// Interaction logic for CounterpartyPickView.xaml
/// </summary>
public partial class CounterpartyPickView : UserControl
{
    public CounterpartyPickView()
    {
        InitializeComponent();
    }
    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Window parentWindow = Window.GetWindow(this);

        parentWindow?.Close();
    }
    private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ConfirmSelection.IsEnabled = true;
    }
}
