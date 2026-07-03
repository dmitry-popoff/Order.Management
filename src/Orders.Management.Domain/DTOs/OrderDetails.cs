namespace Orders.Management.Domain.DTOs;

public sealed class OrderDetails
{
    public long Id { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public decimal Sum { get; init; }
    public long? EmployeeId { get; init; }
    public EmployeeDetails? Employee {  get; init; }
    public long CounterpartyId { get; init; }
    public CounterpartyDetails Counterparty { get; init; }
    public DateTime? DeletedAt { get; init; }
}
