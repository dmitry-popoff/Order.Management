using CommunityToolkit.Mvvm.ComponentModel;

namespace Orders.Management.UI.Models.Counterparties;

public partial class CounterpartyListModel:ObservableObject
{
    [ObservableProperty]
    private long _id;
    [ObservableProperty]
    private string _title;
    [ObservableProperty]
    private string _taxpayerIdentificationNumber;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasCurator))]
    private long? _curatorId;
    
    public bool HasCurator => CuratorId.HasValue;
}
