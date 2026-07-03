namespace Orders.Management.Domain.DTOs;

public class CounterpartyList
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required string TaxpayerIdentificationNumber { get; init; }
    public long? CuratorId { get; init; }
    public DateTime? DeletedAt { get; init; }
}
