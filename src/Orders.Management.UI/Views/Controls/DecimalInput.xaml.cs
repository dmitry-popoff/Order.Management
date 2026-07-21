using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Orders.Management.UI.Views.Controls;

/// <summary>
/// Interaction logic for DecimalInput.xaml
/// </summary>
public partial class DecimalInput : UserControl
{
    //DecimalValue
    public static readonly DependencyProperty DecimalValueProperty =
       DependencyProperty.Register("DecimalValue", typeof(string), typeof(DecimalInput));
    public string DecimalValue
    {
        get
        {
            return (string)GetValue(DecimalValueProperty);
        }
        set
        {
            SetValue(DecimalValueProperty, value);
        }
    }
    public DecimalInput()
    {
        InitializeComponent();
    }
    private void DigitsOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        bool isDigit = IsTextDigitsOnly(e.Text);
        bool isDelimiter = ContainsDelimiter(e.Text);
        if (!isDelimiter)
        {
            e.Handled = !isDigit;
            return;
        }
        e.Handled = !isDelimiter || ContainsDelimiter(((TextBox)sender).Text);
    }
    // Handles pasted clipboard data (e.g., Ctrl+V or right-click paste)
    private void DigitsOnlyTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            string text = (string)e.DataObject.GetData(DataFormats.Text);
            if (!IsTextDigitsOnly(text)
                || Regex.IsMatch(text, "^\\d *\\.?\\d *$") is false
                )
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

    private bool ContainsDelimiter(string text)
    {
        return text.Any(x => IsDelimiter(x));
    }

    private bool IsTextDigitsOnly(string text)
    {
        return text.All(x => Char.IsDigit(x));
    }

    private bool IsDelimiter(char c)
    {
        return c == '.';
    }
}
