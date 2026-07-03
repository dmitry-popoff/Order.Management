using CommunityToolkit.Mvvm.ComponentModel;

namespace Orders.Management.UI.Services;

public interface IDialogService
{
    void ShowMessageBox(string message);

    bool ShowDialog<TViewModel>(TViewModel viewModel)
         where TViewModel : ObservableObject;
}
