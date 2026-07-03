using MediatR;
using Orders.Management.Application.Features.Counterparties.Commands.ChangeCurator;
using Orders.Management.Application.Features.Counterparties.Commands.Create;
using Orders.Management.Application.Features.Counterparties.Commands.Delete;
using Orders.Management.Application.Features.Counterparties.Queries.Find;
using Orders.Management.Application.Features.Counterparties.Queries.GetPage;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.UI.Models.Counterparties;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Counterparties;

internal sealed class CounterpartyService : ICounterpartyService
{
    private readonly ISender _sender;


    public CounterpartyService(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public async ValueTask<Result> Create(CounterpartyDetailsModel counterparty, CancellationToken cancellationToken)
    {
        if (!counterparty.CuratorId.HasValue) return Result.Failure(CounterpartyContracts.Errors.CuratorDoesNotSet);

        var result = await _sender.Send(
            new CreateCounterpartyCommand(
                counterparty.Title,
                counterparty.TaxpayerIdentificationNumber,
                counterparty.CuratorId.Value),
            cancellationToken);

        return result.IsFailure ? Result.Failure(result.Error) : Result.Success();
    }

    public async ValueTask<Result> Delete(long counterpartyId, CancellationToken cancellationToken) => 
        await _sender.Send(new DeleteCounterpartyCommand(counterpartyId), cancellationToken);

    public async ValueTask<Result<CounterpartyDetailsModel>> Find(long counterpartyId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new FindByIdQuery(counterpartyId), cancellationToken);

        return result.IsSuccess ? result.Value.Map() : result.Error;
    }

    public async ValueTask<PagedResult<CounterpartyListModel>> Get(string search, Page page, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPageQuery(page, search), cancellationToken);

        if (result.IsEmpty) return PagedResult<CounterpartyListModel>.Empty;

        return new PagedResult<CounterpartyListModel>
        {
            Page = result.Page,
            Total = result.Total,
            Values = result.Values.Where(x => x != null).Select(x => x.ToListModel()).ToArray()
        };
    }

    public async ValueTask<Result<CounterpartyDetailsModel>> Update(long counterpartyId, long curatorId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ChangeCuratorCommand(counterpartyId, curatorId), cancellationToken);
        
        return result.IsFailure ? result.Error : result.Value.Map();
    }
}