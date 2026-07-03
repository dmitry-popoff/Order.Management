using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Orders;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Orders.Commands.Delete;

internal sealed class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand, Result>
{
    private readonly IStorage<Order> _storage;

    public DeleteOrderCommandHandler(IStorage<Order> storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _storage.DeleteAsync(new FindByIdSpecification<Order> { Id = request.OrderId}, cancellationToken);

        return Result.Success();
    }
}