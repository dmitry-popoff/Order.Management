using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orders.Management.UI.Models;
using System.Collections.ObjectModel;

namespace Orders.Management.UI.ViewModels;

public class NavigationViewModel : ObservableObject
{
    private NavigationItem _selectedNavigationItem;

    public NavigationItem SelectedNavigationItem
    {
        get => _selectedNavigationItem;
        set => SetProperty(ref _selectedNavigationItem, value);
    }

    public AsyncRelayCommand<NavigationItem> NavigateCommand {  get; init; }

    public ObservableCollection<NavigationItem> NavigationItems { get; set; }

    public NavigationViewModel()
    {
        NavigationItems = new ObservableCollection<NavigationItem>
        {
            new NavigationItem 
            { 
                DisplayName = "Employees",
                Icon = "List",
                Uri = "EmployeesView",
                ViewModelType = typeof(EmployeesViewModel)
            },
            new NavigationItem
            {
                DisplayName = "Counterparties",
                Icon = "List",
                Uri = "CounterpartiesView",
                ViewModelType = typeof(CounterpartiesViewModel)
            },
            new NavigationItem
            {
                DisplayName = "Orders",
                Icon = "List",
                Uri = "OrderstView",
                ViewModelType = typeof(OrdersViewModel)
            }
        };
    }
}
