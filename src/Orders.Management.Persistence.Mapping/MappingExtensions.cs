using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Employees;
using Orders.Management.Domain.Entities.Orders;
using Orders.Management.Persistence.Mapping.Entities.Counterparties;
using Orders.Management.Persistence.Mapping.Entities.Employees;
using Orders.Management.Persistence.Mapping.Entities.Orders;

namespace Orders.Management.Persistence.Mapping;

internal static class MappingExtensions
{
    public static EmployeeData Map(this EmployeeDetails employee) => new EmployeeData
    {
        Id = employee.Id,
        Name = employee.Name,
        Surname = employee.Surname,
        Patronymic = employee.Patronymic,
        BirthDate = employee.BirthDate,
        Position = employee.Position
    };

    public static EmployeeData Map(this Employee employee) => new EmployeeData
    {
        Id = employee.Id,
        Name = employee.Name,
        Surname = employee.Surname,
        Patronymic = employee.Patronymic,
        BirthDate = employee.BirthDate,
        Position = employee.Position
    };

    /// <summary>
    /// Factory method for creating from storage
    /// </summary>
    /// <param name="employeeData"></param>
    /// <returns></returns>
    public static Employee Map(this EmployeeData employeeData) => 
        Employee.FromStore(employeeData.Id)
        .WithPatronymic(employeeData.Patronymic)
        .WithSurname(employeeData.Surname)
        .WithPosition(employeeData.Position)
        .WithName(employeeData.Name)
        .WithBirthDate(employeeData.BirthDate)
        .Build()
        .Value;

    public static EmployeeDetails ToDetails(this EmployeeData employeeData) => new EmployeeDetails
    {
        Id = employeeData.Id,
        Surname = employeeData.Surname,
        Name = employeeData.Name,
        Patronymic = employeeData.Patronymic,
        BirthDate = employeeData.BirthDate,
        Position = employeeData.Position
    };

    /// <summary>
    /// Factory method for creating from storage
    /// </summary>
    /// <param name="counterpartyData"></param>
    /// <returns></returns>
    /// 
    public static Counterparty Map(this CounterpartyData counterpartyData) => 
        Counterparty.FromStore(counterpartyData.Id)
            .WithTaxpayerIdentificationNumber(counterpartyData.TaxpayerIdentificationNumber)
            .WithTitle(counterpartyData.Title)
            .WithCurator(counterpartyData.Curator?.Map() ?? Employee.Undefined)
            .Build()
            .Value;

    /// <summary>
    /// Factory method for creating from storage
    /// </summary>
    /// <param name="orderyData"></param>
    /// <returns></returns>
    /// 
    public static Order Map(this OrderData orderyData) =>
        Order.FromStore(orderyData.Id)
        .WithEmployee(orderyData.Employee?.Map())
        .WithCounterparty(orderyData.Counterparty?.Map())
        .WithSum(orderyData.Sum)
        .Build()
        .Value;

    public static CounterpartyDetails ToDetails(this CounterpartyData counterpartyData) => new CounterpartyDetails
    {
        Id = counterpartyData.Id,
        DeletedAt = counterpartyData.DeletedAt,
        Curator = counterpartyData.Curator?.ToDetails(),
        CuratorId = counterpartyData.Curator?.Id,
        Title = counterpartyData.Title,
        TaxpayerIdentificationNumber = counterpartyData.TaxpayerIdentificationNumber
    };

    public static CounterpartyList ToList(this CounterpartyData counterpartyData) => new CounterpartyList
    {
        Id = counterpartyData.Id,
        DeletedAt = counterpartyData.DeletedAt,
        CuratorId = counterpartyData.Curator?.Id,
        Title = counterpartyData.Title,
        TaxpayerIdentificationNumber = counterpartyData.TaxpayerIdentificationNumber
    };

    public static CounterpartyData Map(this CounterpartyDetails counterparty) => new CounterpartyData()
    {
        Id = counterparty.Id,
        CuratorId = counterparty.Curator?.Id,
        Curator = counterparty.Curator?.Map(),
        TaxpayerIdentificationNumber = counterparty.TaxpayerIdentificationNumber,
        Title = counterparty.Title
    };

    public static CounterpartyData Map(this Counterparty counterparty) => new CounterpartyData()
    {
        Id = counterparty.Id,
        CuratorId = counterparty.Curator?.Id,
        Curator = counterparty.Curator?.Map(),
        TaxpayerIdentificationNumber = counterparty.TaxpayerIdentificationNumber,
        Title = counterparty.Title
    };
    public static OrderDetails ToDetails(this OrderData orderData) => new OrderDetails
    {
        Id = orderData.Id,
        DeletedAt = orderData.DeletedAt,
        Counterparty = orderData.Counterparty?.ToDetails(),
        CounterpartyId = orderData.CounterpartyId,
        CreatedAtUtc = orderData.CreatedAtUtc,
        Employee = orderData.Employee?.ToDetails(),
        EmployeeId = orderData.EmployeeId,
        Sum = orderData.Sum
    };
    public static OrderData Map(this OrderDetails order) =>
        new OrderData
        {
            Id = order.Id,
            CreatedAtUtc = order.CreatedAtUtc,
            EmployeeId = order.Employee?.Id,
            CounterpartyId = order.Counterparty.Id,
            Sum = order.Sum
        };

    public static OrderData Map(this Order order) =>
        new OrderData
        {
            Id = order.Id,
            CreatedAtUtc = order.CreatedAtUtc,
            EmployeeId = order.Employee?.Id,
            CounterpartyId = order.Counterparty.Id,
            Sum = order.Sum,
            Employee = order.Employee?.Map(),
            Counterparty = order.Counterparty.Map()
        };
}
