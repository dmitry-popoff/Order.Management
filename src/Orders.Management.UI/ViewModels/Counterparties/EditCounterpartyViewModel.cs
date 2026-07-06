using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models.Counterparties;
using Orders.Management.UI.Services;
using Orders.Management.UI.Services.Counterparties;
using System.ComponentModel.DataAnnotations;

namespace Orders.Management.UI.ViewModels;

public partial class EditCounterpartyViewModel : ObservableValidator
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EmployeePickViewModel _employeePick;
    private readonly IDialogService _dialogService;
    public EditCounterpartyViewModel(IServiceScopeFactory scopeFactory, EmployeePickViewModel employeePick, IDialogService dialogService)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _employeePick = employeePick ?? throw new ArgumentNullException(nameof(employeePick));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }


    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(CreateCounterpartyViewModel), nameof(ValidateCurator))]
    private long _curatorId;
    [ObservableProperty]
    private string _curatorFullName;
    [ObservableProperty]
    private string _curatorPosition;


    public static ValidationResult ValidateCurator(long curatorId, ValidationContext context)
    {
        bool isValid = curatorId > 0;

        if (isValid)
        {
            return ValidationResult.Success;
        }

        return new("curatorId is incorrect");
    }


    public void Init(CounterpartyDetailsModel details)
    {
        Details = details;

        CuratorId = details.CuratorId.HasValue ? details.CuratorId.Value : -1;
        CuratorFullName = details.CuratorFullName ?? string.Empty;
        CuratorPosition = details.CuratorPosition?.ToString() ?? string.Empty;
        Message = string.Empty;
        HasError = false;
        IsSaved = false;
    }

    [ObservableProperty]
    private CounterpartyDetailsModel _details;
    [ObservableProperty]
    private bool _isSaved;
    [ObservableProperty]
    private bool _hasError;
    [ObservableProperty]
    private string _message;
    [ObservableProperty]
    private string _messageColour = "Green";

    private void SetupCounterparty()
    {
        Details.CuratorId = CuratorId;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Details is null) return;

        ValidateAllProperties();

        if (HasErrors)
        {
            HasError = HasErrors;
            Message = "Invalid input";
            MessageColour = "Red";
            return;
        }

        SetupCounterparty();

        using IServiceScope scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetService<ICounterpartyService>();

        if (service is null)
        {
            IsSaved = false;
            Message = "Service not found";
            HasError = true;
            MessageColour = "Red";
            return;
        }

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var result = await service.Update(_details.Id, _curatorId, ctx.Token);

        IsSaved = result.IsSuccess;

        Message = result.IsSuccess ? "Entity saved" : result.Error.ErrorCode;
        HasError = result.IsFailure;
        MessageColour = HasError ? "Red" : "Green";
    }

    [RelayCommand]
    private async void PickCurator()
    {
        _dialogService.ShowDialog(_employeePick);
        if (_employeePick.Current is not null)
        {
            CuratorId = _employeePick.Current.Id;
            CuratorFullName = _employeePick.Current.FullName;
            CuratorPosition = _employeePick.Current.Position.ToString();
        }
        else
        {
            _dialogService.ShowMessageBox("No Employee was selected!");
        }
    }
}