using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models.Orders;
using Orders.Management.UI.Services;
using Orders.Management.UI.Services.Orders;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Orders.Management.UI.ViewModels;

public partial class OrdersViewModel : BaseViewModel
{
    private readonly IDialogService _dialogService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public OrdersViewModel(IDialogService dialogService, IServiceScopeFactory serviceScopeFactory)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    private IAsyncRelayCommand<object> _removeCommand;

    public override IAsyncRelayCommand<object> RemoveCommand =>
        _removeCommand ??= new AsyncRelayCommand<object>(Remove);

    private async Task Remove(object item)
    {
        var entity = item as OrderListModel;

        if (entity is null)
        {
            _dialogService.ShowMessageBox("Nothing selected");
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

        if (service is null) return;

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var result = await service.Delete(entity.Id, ctx.Token);

        if (result.IsSuccess)
        {
            _dialogService.ShowMessageBox($"Deleted successfully {entity.Id}");
            Orders.Remove(entity);
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

        var dialogVM = scope.ServiceProvider.GetRequiredService<CreateOrderViewModel>();

        dialogVM.Init();

        _dialogService.ShowDialog(dialogVM);

        SearchString = dialogVM.CounterpartyTitle;
        await LoadDataAsync();
    }

    private IAsyncRelayCommand<object> _openDetailsCommand;
    public override IAsyncRelayCommand<object> OpenDetailsCommand =>
        _openDetailsCommand ??= new AsyncRelayCommand<object>(OpenDetailsDialog);
    private async Task OpenDetailsDialog(object item)
    {
        var entity = item as OrderListModel;

        if (entity is null)
        {
            _dialogService.ShowMessageBox("Nothing selected");
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var details = await service.Find(entity.Id, ctx.Token);

        if (details.IsFailure) return;

        var dialogVM = scope.ServiceProvider.GetRequiredService<EditOrderViewModel>();

        var result = ProcessEdit(details.Value, dialogVM);

        if (result == true)
        {
            SearchString = dialogVM.CounterpartyTitle;
            await LoadDataAsync();
        }
    }

    private bool ProcessEdit(OrderDetailsModel detailsModel, EditOrderViewModel viewModel)
    {
        viewModel.Init(detailsModel);

        return _dialogService.ShowDialog(viewModel);
    }

    private IAsyncRelayCommand _loadDataCommand;
    public override IAsyncRelayCommand LoadDataCommand =>
        _loadDataCommand ??= new AsyncRelayCommand(LoadDataAsync);
    private async Task LoadDataAsync()
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        string search = SearchString ?? string.Empty;

        var result = string.IsNullOrWhiteSpace(search)
            ? await service.Get(CurrentPage, ctx.Token)
            : await service.Get(CurrentPage, search, ctx.Token);

        Orders = new ObservableCollection<OrderListModel>(result.Values);
        SelectedItem = Orders.FirstOrDefault()!;
        CurrentPage = result.Page;
        TotalItems = result.Total;
        SetupNavigation();
        InitViewSource();
    }
    private void InitViewSource()
    {
        ViewSource = (ListCollectionView)CollectionViewSource.GetDefaultView(Orders);
        ViewSource.SortDescriptions.Add(new SortDescription("CounterpartyTitle", ListSortDirection.Ascending));
        ViewSource.IsLiveSorting = true;
        ViewSource.LiveSortingProperties.Add("CounterpartyTitle");
        ViewSource.Filter = ViewSourceFilter;
        ViewSource.Refresh();
    }

    private bool ViewSourceFilter(object item)
    {
        if (item is OrderListModel)
        {
            OrderListModel listItem = item as OrderListModel;

            return string.IsNullOrEmpty(SearchString) || string.IsNullOrWhiteSpace(SearchString) ?
                true : listItem.CounterpartyTitle.ToLower().StartsWith(SearchString.ToLower());
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
    private OrderListModel _selectedItem;
    [ObservableProperty]
    private string _searchString;

    public ObservableCollection<OrderListModel> Orders { get; set; }
}

