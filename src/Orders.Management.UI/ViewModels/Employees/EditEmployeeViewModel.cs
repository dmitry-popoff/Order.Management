using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orders.Management.Domain.Entities.Employees;
using Orders.Management.UI.Models.Employees;
using Orders.Management.UI.Services.Employees;
using System.ComponentModel.DataAnnotations;
using static Orders.Management.Domain.Entities.Employees.EmployeeContracts;

namespace Orders.Management.UI.ViewModels;

public partial class EditEmployeeViewModel: ObservableValidator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EditEmployeeViewModel(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(EditEmployeeViewModel), nameof(ValidateName))]
    private string _name;
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(EditEmployeeViewModel), nameof(ValidateName))]
    private string _surname;
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(EditEmployeeViewModel), nameof(ValidateName))]
    private string _patronymic;
    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(EditEmployeeViewModel), nameof(ValidatePosition))]
    private PositionType _position;
    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(EditEmployeeViewModel), nameof(ValidateBirthdate))]
    private DateTime _birthDate;

    public static ValidationResult ValidateBirthdate(DateTime date, ValidationContext context)
    {
        var instance = (EditEmployeeViewModel)context.ObjectInstance;
        bool isValid = EmployeeValidator.ValidateBirthDate(date, out var error);

        if (isValid)
        {
            return ValidationResult.Success;
        }

        return new(error?.Message ?? "BirthDate is incorrect");
    }

    public static ValidationResult ValidatePosition(PositionType position, ValidationContext context)
    {
        var instance = (EditEmployeeViewModel)context.ObjectInstance;
        bool isValid = EmployeeValidator.ValidatePosition(position, out var error);

        if (isValid)
        {
            return ValidationResult.Success;
        }

        return new(error?.Message ?? "position is incorrect");
    }

    public static ValidationResult ValidateName(string name, ValidationContext context)
    {
        var instance = (EditEmployeeViewModel)context.ObjectInstance;
        bool isValid = EmployeeValidator.ValidateName(name, out var error);

        if (isValid)
        {
            return ValidationResult.Success;
        }

        return new(error?.Message ?? "name has incorrect format");
    }

    public enum OperationType { Update = 0, Create }

    public OperationType CurrentOperation {  get; private set; }

    public void SetEmployee(EmployeeDetailsModel employee, OperationType operation = default)
    {
        Employee = employee;
        CurrentOperation = operation;

        Name = employee.Name;
        Surname = employee.Surname;
        Patronymic = employee.Patronymic;
        BirthDate = employee.BirthDate;
        Position = employee.Position;

        Message = string.Empty;
        HasError = false;
        IsSaved = false;
    }

    [ObservableProperty]
    private EmployeeDetailsModel _employee;
    [ObservableProperty]
    private bool _isSaved;
    [ObservableProperty]
    private bool _hasError;
    [ObservableProperty]
    private string _message;
    [ObservableProperty]
    private string _messageColour = "Green";

    private void SetupEmployee()
    {
        Employee.Name = Name;
        Employee.Surname = Surname;
        Employee.Patronymic = Patronymic;
        Employee.Position = Position;
        Employee.BirthDate = BirthDate;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Employee is null) return;

        ValidateAllProperties();

        if (HasErrors)
        {
            HasError = HasErrors;
            Message = "Invalid input";
            MessageColour = "Red";
            return;
        }

        SetupEmployee();

        using IServiceScope scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetService<IEmployeeService>();

        if (service is null)
        {
            IsSaved = false;
            Message = "Service not found"; 
            HasError = true;
            MessageColour = "Red"; 
            return;
        }

        using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var result = CurrentOperation == OperationType.Update
            ? await service.Update(Employee, ctx.Token)
            : await service.Create(Employee, ctx.Token);

        IsSaved = result.IsSuccess;

        Message = result.IsSuccess ? "Entity saved" : result.Error.ErrorCode;
        HasError = result.IsFailure;
        MessageColour = HasError? "Red": "Green";
    }
}

