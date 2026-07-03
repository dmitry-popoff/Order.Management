using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models.Counterparties;
using Orders.Management.UI.Services.Counterparties;
using Shared.Abstractions;
using System.Collections.ObjectModel;

namespace Orders.Management.UI.ViewModels.Counterparties;

public partial class CounterpartyPickViewModel: BaseViewModel
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public CounterpartyPickViewModel(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        CurrentPage = Page.GetPage(1, 10);
    }

    [ObservableProperty]
    private CounterpartyListModel _current;

    [ObservableProperty]
    private string _searchString;
    public ObservableCollection<CounterpartyListModel> Entities { get; set; } = new();

    private IAsyncRelayCommand _loadDataCommand;
    public override IAsyncRelayCommand LoadDataCommand =>
        _loadDataCommand ??= new AsyncRelayCommand(LoadDataAsync);

    private async Task LoadDataAsync()
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICounterpartyService>();

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var result = await service.Get(SearchString, CurrentPage, ctx.Token);

        Entities.Clear();

        foreach (var item in result.Values)
        {
            Entities.Add(item);
        }
        Current = Entities.FirstOrDefault()!;
        CurrentPage = result.Page;
        TotalItems = result.Total;
        SetupNavigation();
    }
}
