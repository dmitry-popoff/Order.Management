using System.Windows;
using System.Windows.Controls;

namespace Orders.Management.UI.Views
{
    /// <summary>
    /// Interaction logic for EmployeePickView.xaml
    /// </summary>
    public partial class EmployeePickView : UserControl
    {
        public EmployeePickView()
        {
            InitializeComponent();
            ConfirmSelection.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);

            parentWindow?.Close();
        }
        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfirmSelection.IsEnabled = true;
        }
    }
}
