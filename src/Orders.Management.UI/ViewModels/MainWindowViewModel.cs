using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models;

namespace Orders.Management.UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _navigation = new NavigationViewModel
        {
            NavigateCommand = new AsyncRelayCommand<NavigationItem>(HandleLoad)
        };
        _listViewModels.Add(typeof(EmployeesViewModel), _serviceProvider.GetRequiredService<EmployeesViewModel>);
        _listViewModels.Add(typeof(CounterpartiesViewModel), _serviceProvider.GetRequiredService<CounterpartiesViewModel>);
        _listViewModels.Add(typeof(OrdersViewModel), _serviceProvider.GetRequiredService<OrdersViewModel>);
    }
    [ObservableProperty]
    private BaseViewModel _currentViewModel;

    [ObservableProperty]
    private NavigationViewModel _navigation;

    private async Task HandleLoad(NavigationItem navigationItem)
    {
        CurrentViewModel = _listViewModels[navigationItem.ViewModelType].Invoke();

        await CurrentViewModel.LoadDataCommand.ExecuteAsync(null);

        await Task.Delay(500);
    }

    private readonly Dictionary<Type, Func<BaseViewModel>> _listViewModels = new();
}
