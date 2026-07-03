using CommunityToolkit.Mvvm.ComponentModel;
using Orders.Management.Domain.Entities.Employees;

namespace Orders.Management.UI.Models.Employees;

public partial class EmployeeDetailsModel : ObservableObject
{
    [ObservableProperty]
    private long _id;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _surname;
    [ObservableProperty]
    private string _patronymic;
    [ObservableProperty]
    private PositionType _position;
    [ObservableProperty]
    private DateTime _birthDate;


    public override string ToString()
    {
        return string.Join(' ', Surname, Name, Patronymic);
    }
}
