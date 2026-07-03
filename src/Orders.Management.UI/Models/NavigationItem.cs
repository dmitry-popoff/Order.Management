using CommunityToolkit.Mvvm.ComponentModel;

namespace Orders.Management.UI.Models;

public partial class NavigationItem : ObservableObject
{
    [ObservableProperty]
    private string _displayName;
    [ObservableProperty]
    private string _uri;
    [ObservableProperty]
    private string _icon;

    public required Type ViewModelType {  get; set; }
}