using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Orders.Management.UI.Views;

/// <summary>
/// Interaction logic for EditOrderView.xaml
/// </summary>
public partial class EditOrderView : UserControl
{
    public EditOrderView()
    {
        InitializeComponent();
    }
    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Window parentWindow = Window.GetWindow(this);

        parentWindow?.Close();
    }
    private void DigitsOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Set Handled to true if the text contains anything other than letters
        e.Handled = !IsTextDigitsOnly(e.Text);
    }
    // Handles pasted clipboard data (e.g., Ctrl+V or right-click paste)
    private void DigitsOnlyTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            string text = (string)e.DataObject.GetData(DataFormats.Text);
            if (!IsTextDigitsOnly(text))
            {
                // Cancel the paste operation if it contains invalid characters
                e.CancelCommand();
            }
        }
        else
        {
            // Cancel if the pasted item is not text (like an image)
            e.CancelCommand();
        }
    }
    private bool IsTextDigitsOnly(string text)
    {
        return text.All(x => Char.IsDigit(x));
    }
}
