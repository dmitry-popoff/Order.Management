using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Orders.Management.UI.Views;

public partial class EditEmployeeView : UserControl
{
    public EditEmployeeView()
    {
        InitializeComponent();
    }
    private void LettersOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Set Handled to true if the text contains anything other than letters
        e.Handled = !IsTextLettersOnly(e.Text);
    }

    // Handles pasted clipboard data (e.g., Ctrl+V or right-click paste)
    private void LettersOnlyTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            string text = (string)e.DataObject.GetData(DataFormats.Text);
            if (!IsTextLettersOnly(text))
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

    // Helper method using regex to check for letters only
    private bool IsTextLettersOnly(string text)
    {
        // ^[a-zA-Z]+$ matches only English uppercase and lowercase letters
        // Use ^[\p{L}]+$ instead if you want to support global/accented letters (like é, ü, ñ)
        return Regex.IsMatch(text, "^[\\p{L}]+$");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Window parentWindow = Window.GetWindow(this);

        parentWindow?.Close();
    }
}
