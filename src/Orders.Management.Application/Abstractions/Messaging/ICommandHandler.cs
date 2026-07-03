using MediatR;

namespace Orders.Management.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where  TCommand : ICommand<TResponse>
{
}