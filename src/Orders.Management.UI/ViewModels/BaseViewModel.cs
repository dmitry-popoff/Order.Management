using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Abstractions;


namespace Orders.Management.UI.ViewModels;

public partial class BaseViewModel: ObservableObject, IDataLoader 
{
    public BaseViewModel() 
    {
        _previousPageCommand = new AsyncRelayCommand(LoadPreviousPageAsync);
        _nextPageCommand = new AsyncRelayCommand(LoadNextPageAsync);
    }

    public virtual IAsyncRelayCommand<object> RemoveCommand { get; set; }
    public virtual IAsyncRelayCommand LoadDataCommand { get; set; }

    public virtual IAsyncRelayCommand<object> OpenDetailsCommand { get; set; }
    public virtual IRelayCommand OpenEditorCommand { get; set; }

    private IAsyncRelayCommand _nextPageCommand;
    public IAsyncRelayCommand NextPageCommand => _nextPageCommand;
    protected async Task LoadNextPageAsync()
    {
        CurrentPage = CurrentPage.Next();
        await LoadDataCommand.ExecuteAsync(null);
    }

    private IAsyncRelayCommand _previousPageCommand;
    public IAsyncRelayCommand PreviousPageCommand => _previousPageCommand;
    protected async Task LoadPreviousPageAsync()
    {
        CurrentPage = CurrentPage.Prev();
        await LoadDataCommand.ExecuteAsync(null);
    }

    protected void SetupNavigation() => 
        (HasNextPage, HasPrevPage) = (TotalItems > CurrentPage.Size * CurrentPage.Number, CurrentPage.Number > 1);

    [ObservableProperty]
    private Page _currentPage = Page.Default;
    [ObservableProperty]
    private int _totalItems;
    [ObservableProperty]
    private bool _hasNextPage;
    [ObservableProperty]
    private bool _hasPrevPage;
}

