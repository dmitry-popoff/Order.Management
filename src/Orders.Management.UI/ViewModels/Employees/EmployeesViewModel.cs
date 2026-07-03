using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models.Employees;
using Orders.Management.UI.Services;
using Orders.Management.UI.Services.Employees;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Orders.Management.UI.ViewModels;

public partial class EmployeesViewModel: BaseViewModel
{
    private readonly IDialogService _dialogService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public EmployeesViewModel(IDialogService dialogService, IServiceScopeFactory serviceScopeFactory)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    private IAsyncRelayCommand<object> _removeCommand;

    public override IAsyncRelayCommand<object> RemoveCommand =>
        _removeCommand ??= new AsyncRelayCommand<object>(Remove);

    private async Task Remove(object item)
    {
        var employee = item as EmployeeListModel;

        if (employee is null)
        {
            _dialogService.ShowMessageBox("Nothing selected");
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

        if (service is null) return;

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var result = await service.Delete(employee.Id, ctx.Token);

        if (result.IsSuccess)
        {
            _dialogService.ShowMessageBox($"Deleted successfully {employee.Id}");
            Employees.Remove(employee);
            InitViewSource();
        }
        else
        {
            _dialogService.ShowMessageBox(result.Error.Message);
        }
    }

    private IRelayCommand _openEditorCommand;
    public override IRelayCommand OpenEditorCommand =>
        _openEditorCommand ??= new AsyncRelayCommand(OpenEditorDialog);

    private async Task OpenEditorDialog()
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var dialogVM = scope.ServiceProvider.GetRequiredService<EditEmployeeViewModel>();

        var result = ProcessEdit(new EmployeeDetailsModel { BirthDate = DateTime.UtcNow}, dialogVM, EditEmployeeViewModel.OperationType.Create);
    }

    private IAsyncRelayCommand<object> _openDetailsCommand;
    public override IAsyncRelayCommand<object> OpenDetailsCommand => 
        _openDetailsCommand ??= new AsyncRelayCommand<object>(OpenDetailsDialog);
    private async Task OpenDetailsDialog(object item)
    {
        var employee = item as EmployeeListModel;

        if (employee is null)
        {
            _dialogService.ShowMessageBox("Nothing selected");
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var employeeDetails = await employeeService.Find(employee.Id, ctx.Token);

        if (employeeDetails.IsFailure) return;

        var dialogVM = scope.ServiceProvider.GetRequiredService<EditEmployeeViewModel>();

        var result = ProcessEdit(employeeDetails.Value, dialogVM);

        if (result == true)
        {
            SelectedItem = Employees.FirstOrDefault(e => e.Id == employee.Id);
            SelectedItem?.FullName = dialogVM.Employee.ToString();
            SelectedItem?.Position = dialogVM.Employee.Position;
        }
    }

    private bool ProcessEdit(
        EmployeeDetailsModel detailsModel,
        EditEmployeeViewModel viewModel,
        EditEmployeeViewModel.OperationType operation = default
        )
    {
        viewModel.SetEmployee(detailsModel, operation);

        return _dialogService.ShowDialog(viewModel);
    }

    private IAsyncRelayCommand _loadDataCommand;
    public override IAsyncRelayCommand LoadDataCommand => 
        _loadDataCommand ??= new AsyncRelayCommand(LoadDataAsync);
    private async Task LoadDataAsync()
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var employeesService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var result = await employeesService.Get(SearchString, CurrentPage, ctx.Token);

        Employees = new ObservableCollection<EmployeeListModel>(result.Values);
        SelectedItem = Employees.FirstOrDefault()!;
        CurrentPage = result.Page;
        TotalItems = result.Total;
        SetupNavigation();
        InitViewSource();
    }
    private void InitViewSource()
    {
        ViewSource = (ListCollectionView)CollectionViewSource.GetDefaultView(Employees);
        ViewSource.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));
        ViewSource.IsLiveSorting = true;
        ViewSource.LiveSortingProperties.Add("FullName");
        ViewSource.Filter = ViewSourceFilter;
        ViewSource.Refresh();
    }

    private bool ViewSourceFilter(object item)
    {
        if (item is EmployeeListModel)
        {
            EmployeeListModel todoItem = item as EmployeeListModel;

            return string.IsNullOrEmpty(SearchString) || string.IsNullOrWhiteSpace(SearchString) ?
                true : todoItem.FullName.ToLower().StartsWith(SearchString.ToLower());
        }
        else
        {
            return true;
        }
    }

    private RelayCommand _filterCommand;
    public RelayCommand FilterCommand => _filterCommand ??= new RelayCommand(ExecuteFilterCommand);

    private void ExecuteFilterCommand()
    {
        ViewSource.Refresh();
    }

    [ObservableProperty]
    private ListCollectionView _viewSource;
    [ObservableProperty]
    private EmployeeListModel _selectedItem;
    [ObservableProperty]
    private string _searchString;

    public ObservableCollection<EmployeeListModel> Employees { get; set; }
}

