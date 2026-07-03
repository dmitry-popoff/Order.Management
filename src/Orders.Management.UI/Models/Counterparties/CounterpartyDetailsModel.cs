using CommunityToolkit.Mvvm.ComponentModel;

namespace Orders.Management.UI.Models.Counterparties;

public partial class CounterpartyDetailsModel : ObservableObject
{
    [ObservableProperty]
    private long _id;
    [ObservableProperty]
    private string _title;
    [ObservableProperty]
    private string _taxpayerIdentificationNumber;
    [ObservableProperty]
    private string? _curatorFullName;
    [ObservableProperty]
    private string? _curatorPosition;
    [ObservableProperty]
    private long? _curatorId;
}
