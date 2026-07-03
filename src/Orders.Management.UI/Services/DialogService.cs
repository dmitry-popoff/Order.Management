using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Orders.Management.UI.Services;

public class DialogService : IDialogService
{
    public bool ShowDialog<TViewModel>(TViewModel viewModel) 
        where TViewModel : ObservableObject
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            Window dialogWindow = new Window
            {
                Content = viewModel, // WPF uses DataTemplates to find the UserControl
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = System.Windows.Application.Current.MainWindow,
                IsEnabled = true,
                IsHitTestVisible = true,
                Height = 400
            };
        

            dialogWindow.Activate();

            dialogWindow.ShowDialog();
        });

        return true;
    }

    public void ShowMessageBox(string message)
    {
        MessageBox.Show(message);
    }
}