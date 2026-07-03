using CommunityToolkit.Mvvm.ComponentModel;
using Orders.Management.UI.Models.Counterparties;
using Orders.Management.UI.Models.Employees;

namespace Orders.Management.UI.Models.Orders;

public partial class OrderDetailsModel : ObservableObject
{
    [ObservableProperty]
    private long _id;
    [ObservableProperty]
    private decimal _sum;
    [ObservableProperty]
    private long _counterpartyId;
    [ObservableProperty]
    private CounterpartyDetailsModel _counterparty;
    [ObservableProperty]
    private long? _employeeId;
    [ObservableProperty]
    private EmployeeDetailsModel? _employee;
    [ObservableProperty]
    private DateTime _createdAt;
}
