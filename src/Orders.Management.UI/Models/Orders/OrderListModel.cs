using CommunityToolkit.Mvvm.ComponentModel;

namespace Orders.Management.UI.Models.Orders;

public partial class OrderListModel: ObservableObject
{
    [ObservableProperty]
    private long _id;
    [ObservableProperty]
    private decimal _sum;
    [ObservableProperty]
    private long _counterpartyId;
    [ObservableProperty]
    private string _counterpartyTitle;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasEmployee))]
    private long? _employeeId;
    [ObservableProperty]
    private DateTime _createdAt;

    public bool HasEmployee => EmployeeId.HasValue;
}
