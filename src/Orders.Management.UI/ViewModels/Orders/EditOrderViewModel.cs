using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.UI.Models.Orders;
using Orders.Management.UI.Services;
using Orders.Management.UI.Services.Orders;
using System.ComponentModel.DataAnnotations;
using static Orders.Management.Domain.Entities.Orders.OrderContracts;

namespace Orders.Management.UI.ViewModels;

public partial class EditOrderViewModel: ObservableValidator
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EmployeePickViewModel _employeePick;
    private readonly IDialogService _dialogService;
    public EditOrderViewModel(
        IServiceScopeFactory scopeFactory,
        EmployeePickViewModel employeePick,
        IDialogService dialogService)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _employeePick = employeePick ?? throw new ArgumentNullException(nameof(employeePick));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(EditOrderViewModel), nameof(ValidateSum))]
    private decimal _sum;
    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(EditOrderViewModel), nameof(ValidateRefId))]
    private long _employeeId;
    [Required]
    [ObservableProperty]
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

    public static ValidationResult ValidateSum(decimal sum, ValidationContext context)
    {
        var instance = (EditOrderViewModel)context.ObjectInstance;
        bool isValid = OrderValidator.ValidateSum(sum, out var error);

        if (isValid)
        {
            return ValidationResult.Success;
        }
        return new(error?.Message ?? "Order sum is incorrect");
    }


    public void Init(OrderDetailsModel order)
    {
        Details = order;
        Sum = order.Sum;
        CounterpartyTitle = order.Counterparty.Title;
        TaxpayerId = order.Counterparty.TaxpayerIdentificationNumber;
        EmployeeId = order.Employee?.Id ?? -1;
        CounterpartyId = order.Counterparty.Id;
        EmployeeName = order.Employee?.ToString() ?? "Undefined";

        Message = string.Empty;
        HasError = false;
        IsSaved = false;
    }

    [ObservableProperty]
    private OrderDetailsModel _details;
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
        Details.Sum = Sum;
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

        var result = await service.Update(Details.Id, EmployeeId, Sum, ctx.Token);

        IsSaved = result.IsSuccess;

        Message = result.IsSuccess ? "Entity saved" : result.Error.ErrorCode;
        HasError = result.IsFailure;
        MessageColour = HasError ? "Red" : "Green";
    }

    [RelayCommand]
    private async void PickEmployee()
    {
        _dialogService.ShowDialog(_employeePick);

        Details.EmployeeId = _employeePick.Current.Id;
        EmployeeName = _employeePick.Current.FullName;
        EmployeeId = _employeePick.Current.Id;
    }    
}

