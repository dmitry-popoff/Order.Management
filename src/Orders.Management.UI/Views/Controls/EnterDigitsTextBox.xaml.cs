using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Orders.Management.UI.Views.Controls;

/// <summary>
/// Interaction logic for EnterDigitsTextBox.xaml
/// </summary>
public partial class EnterDigitsTextBox : UserControl
{
    public static readonly DependencyProperty DigitsTextProperty =
       DependencyProperty.Register("DigitsText", typeof(string), typeof(EnterDigitsTextBox));
    public string DigitsText
    {
        get
        {
            return (string)GetValue(DigitsTextProperty);
        }
        set
        {
            SetValue(DigitsTextProperty, value);
        }
    }
    public EnterDigitsTextBox()
    {
        InitializeComponent();
    }
    private void DigitsOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Set Handled to true if the text contains anything other than letters
        e.Handled = !IsTextDigitsOnly(e.Text);
    }

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

    // Helper method using regex to check for letters only
    private bool IsTextDigitsOnly(string text)
    {
        return text.All(x => Char.IsDigit(x));
    }
}
