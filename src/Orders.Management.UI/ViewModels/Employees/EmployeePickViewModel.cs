using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models.Employees;
using Orders.Management.UI.Services.Employees;
using Shared.Abstractions;
using System.Collections.ObjectModel;

namespace Orders.Management.UI.ViewModels;

public partial class EmployeePickViewModel: BaseViewModel
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public EmployeePickViewModel(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        CurrentPage = Page.GetPage(1, 10);
    }

    [ObservableProperty]
    private EmployeeListModel _current;

    [ObservableProperty]
    private string _searchString;
    public ObservableCollection<EmployeeListModel> Employees { get; set; } = new();

    private IAsyncRelayCommand _loadDataCommand;
    public override IAsyncRelayCommand LoadDataCommand =>
        _loadDataCommand ??= new AsyncRelayCommand(LoadDataAsync);

    private async Task LoadDataAsync()
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var employeesService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var result = await employeesService.Get(SearchString, CurrentPage, ctx.Token);

        Employees.Clear(); 
        
        foreach (var item in result.Values)
        {
            Employees.Add(item);
        }
        Current = Employees.FirstOrDefault()!;
        CurrentPage = result.Page;
        TotalItems = result.Total;
        SetupNavigation();
    }
}
