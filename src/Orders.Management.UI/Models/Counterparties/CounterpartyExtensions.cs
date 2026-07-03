using Orders.Management.Domain.DTOs;

namespace Orders.Management.UI.Models.Counterparties;

public static class CounterpartyExtensions
{
    public static CounterpartyDetailsModel Map(this CounterpartyDetails counterparty) =>
        new CounterpartyDetailsModel
        {
            Id = counterparty.Id,
            CuratorFullName = counterparty.Curator?.FullName,
            CuratorPosition = counterparty.Curator?.Position.ToString(),
            CuratorId = counterparty.Curator?.Id,
            Title = counterparty.Title,
            TaxpayerIdentificationNumber = counterparty.TaxpayerIdentificationNumber
        };

    public static CounterpartyListModel ToListModel(this CounterpartyList counterparty) =>
        new CounterpartyListModel
        {
            Id = counterparty.Id,
            Title = counterparty.Title,
            TaxpayerIdentificationNumber = counterparty.TaxpayerIdentificationNumber,
            CuratorId = counterparty.CuratorId
        };

}
