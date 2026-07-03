using CommunityToolkit.Mvvm.ComponentModel;
using Orders.Management.Domain.Entities.Employees;

namespace Orders.Management.UI.Models.Employees;

public partial class EmployeeListModel: ObservableObject
{
    [ObservableProperty]
    private long _id;
    [ObservableProperty]
    private string _fullName;
    [ObservableProperty]
    private PositionType _position;
}
