using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models.Orders;
using Orders.Management.UI.Services;
using Orders.Management.UI.Services.Orders;
using Orders.Management.UI.ViewModels.Counterparties;
using System.ComponentModel.DataAnnotations;
using static Orders.Management.Domain.Entities.Orders.OrderContracts;

namespace Orders.Management.UI.ViewModels;

public partial class CreateOrderViewModel : ObservableValidator
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EmployeePickViewModel _employeePick;
    private readonly CounterpartyPickViewModel _counterpartePick;
    private readonly IDialogService _dialogService;
    public CreateOrderViewModel(
        IServiceScopeFactory scopeFactory,
        EmployeePickViewModel employeePick,
        IDialogService dialogService,
        CounterpartyPickViewModel counterpartePick)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _employeePick = employeePick ?? throw new ArgumentNullException(nameof(employeePick));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _counterpartePick = counterpartePick ?? throw new ArgumentNullException(nameof(counterpartePick));
    }

    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(CreateOrderViewModel), nameof(ValidateSum))]
    private string _sum;
    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(CreateOrderViewModel), nameof(ValidateRefId))]
    private long _employeeId;
    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(CreateOrderViewModel), nameof(ValidateRefId))]
    private long _counterpartyId;
    [Required]
    [ObservableProperty]
    private string _counterpartyTitle;
    [Required]
    [ObservableProperty]
    private string _taxpayerId;
    [Required]
    [ObservableProperty]
    private string _employeeName;

    public static ValidationResult ValidateRefId(long refId, ValidationContext context)
    {
        bool isValid = refId > 0;

        if (isValid)
        {
            return ValidationResult.Success;
        }
        return new("Reference Id is incorrect");
    }

    public static ValidationResult ValidateSum(string sumStr, ValidationContext context)
    {
        bool isValid = decimal.TryParse(sumStr, out decimal sum);
        if (!isValid) return new ValidationResult("Invalid decimal format!");

        var instance = (CreateOrderViewModel)context.ObjectInstance;
        isValid = OrderValidator.ValidateSum(sum, out var error);

        if (isValid)
        {
            return ValidationResult.Success;
        }
        return new(error?.Message ?? "Order sum is incorrect");
    }


    public void Init()
    {
        Details = new OrderListModel();

        CounterpartyTitle = string.Empty;
        TaxpayerId = string.Empty;
        EmployeeId = -1;
        CounterpartyId = -1;

        Message = string.Empty;
        HasError = false;
        IsSaved = false;
    }

    [ObservableProperty]
    private OrderListModel _details;
    [ObservableProperty]
    private bool _isSaved;
    [ObservableProperty]
    private bool _hasError;
    [ObservableProperty]
    private string _message;
    [ObservableProperty]
    private string _messageColour = "Green";

    private void SetupEntity()
    {
        Details.Sum = decimal.Parse(Sum);
        Details.CounterpartyId = CounterpartyId;
        Details.EmployeeId = EmployeeId;
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

        SetupEntity();

        using IServiceScope scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetService<IOrderService>();

        if (service is null)
        {
            IsSaved = false;
            Message = "Service not found";
            HasError = true;
            MessageColour = "Red";
            return;
        }

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var result = await service.Create(Details, ctx.Token);

        IsSaved = result.IsSuccess;

        Message = result.IsSuccess ? "Entity saved" : result.Error.ErrorCode;
        HasError = result.IsFailure;
        MessageColour = HasError ? "Red" : "Green";
    }

    [RelayCommand]
    private async void PickEmployee()
    {
        _dialogService.ShowDialog(_employeePick);

        if (_employeePick.Current is not null)
        {

            Details.EmployeeId = _employeePick.Current.Id;
            EmployeeName = _employeePick.Current.FullName;
            EmployeeId = _employeePick.Current.Id;
        }
        else
        {
            _dialogService.ShowMessageBox("No Employee was selected!");
        }        
    }

    [RelayCommand]
    private async void PickCounterparty()
    {
        _dialogService.ShowDialog(_counterpartePick);

        if (_counterpartePick.Current is not null)
        {
            Details.CounterpartyId = _counterpartePick.Current.Id;
            CounterpartyTitle = _counterpartePick.Current.Title;
            TaxpayerId = _counterpartePick.Current.TaxpayerIdentificationNumber;
            CounterpartyId = _counterpartePick.Current.Id;
        }
        else
        {
            _dialogService.ShowMessageBox("No Counterparty was selected!");
        }
    }
}

