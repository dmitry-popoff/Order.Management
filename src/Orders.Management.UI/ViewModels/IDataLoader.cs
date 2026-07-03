using CommunityToolkit.Mvvm.Input;

namespace Orders.Management.UI.ViewModels;

public interface IDataLoader
{
    IAsyncRelayCommand LoadDataCommand{ get; }
}

