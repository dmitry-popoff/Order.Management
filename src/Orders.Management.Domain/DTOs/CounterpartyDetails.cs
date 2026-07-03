namespace Orders.Management.Domain.DTOs;

public class CounterpartyDetails
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required string TaxpayerIdentificationNumber { get; init; }
    public long? CuratorId { get; init; }
    public EmployeeDetails? Curator { get; init; }
    public DateTime? DeletedAt { get; init; }
}
