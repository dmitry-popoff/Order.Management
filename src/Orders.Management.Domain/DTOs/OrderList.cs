namespace Orders.Management.Domain.DTOs;

public sealed class OrderList
{
    public long Id { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public decimal Sum { get; init; }
    public long? EmployeeId { get; init; }
    public long CounterpartyId { get; init; }
    public string? CounterpartyTitle {  get; init; }
    public DateTime? DeletedAt { get; init; }
}
