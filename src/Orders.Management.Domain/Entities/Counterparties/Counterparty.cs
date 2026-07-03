using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees;
using Shared;

namespace Orders.Management.Domain.Entities.Counterparties;

internal partial class Counterparty : EntityBase
{
    /// <summary>
    /// Factory method for creating new object
    /// </summary>
    /// <param name="title"></param>
    /// <param name="taxpayerIdentificationNumber"></param>
    /// <param name="curator"></param>
    /// <returns></returns>
    public static Result<Counterparty> Create(string title, string taxpayerIdentificationNumber, Employee curator) 
        => new CounterpartyBuilder()
            .WithTitle(title)
            .WithCurator(curator)
            .WithTaxpayerIdentificationNumber(taxpayerIdentificationNumber)
            .Build();

    private Counterparty(long counterpartyId, string title, string taxpayerIdentificationNumber, Employee curator)
        :  this(title, taxpayerIdentificationNumber, curator)
    {
        Id = counterpartyId;
    }

    private Counterparty(string title, string taxpayerIdentificationNumber, Employee curator)
    {
        Title = title;
        TaxpayerIdentificationNumber = taxpayerIdentificationNumber;
        Curator = curator;
    }

    public string Title { get; private set; }
    public string TaxpayerIdentificationNumber { get; private set; }
    public Employee Curator { get; private set; }
    public Result<Counterparty> ChangeCurator(Employee curator)
    {
        if (CheckCurator() is false) return CounterpartyContracts.Errors.CuratorDoesNotSet;

        Curator = curator;
        return this;
    }
    public bool CheckCurator() => Curator is not null && Curator.Equals(Employee.Undefined) is false;

    public CounterpartyDetails ToDetails() =>
        new CounterpartyDetails
        {
            Id = Id,
            CuratorId = Curator.Id,
            Curator = Curator?.ToDetails(),
            TaxpayerIdentificationNumber = TaxpayerIdentificationNumber,
            Title = Title
        };
}