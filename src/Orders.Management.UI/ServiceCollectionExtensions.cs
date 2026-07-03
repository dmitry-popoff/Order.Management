using Microsoft.Extensions.Logging;
using Orders.Management.UI;
using Orders.Management.UI.Services;
using Orders.Management.UI.Services.Counterparties;
using Orders.Management.UI.Services.Employees;
using Orders.Management.UI.Services.Orders;
using Orders.Management.UI.ViewModels;
using Orders.Management.UI.ViewModels.Counterparties;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<EmployeesViewModel>();
        services.AddSingleton<EditEmployeeViewModel>();
        services.AddSingleton<CounterpartiesViewModel>();
        services.AddSingleton<CreateCounterpartyViewModel>();
        services.AddSingleton<EditCounterpartyViewModel>();
        services.AddSingleton<CounterpartyPickViewModel>();
        services.AddSingleton<EmployeePickViewModel>();
        services.AddSingleton<OrdersViewModel>();
        services.AddSingleton<CreateOrderViewModel>();
        services.AddSingleton<EditOrderViewModel>();
        
        services.AddServices();

        return services;
    }

    internal static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<EmployeeService>();
        services.AddTransient<IEmployeeService, ExceptionlessEmployeeService>(sp =>
        {
            var service = sp.GetRequiredService<EmployeeService>();
            var logger = sp.GetRequiredService<ILogger<ExceptionlessEmployeeService>>();
            return new ExceptionlessEmployeeService(service, logger);
        });
        services.AddTransient<CounterpartyService>();
        services.AddTransient<ICounterpartyService, ExceptionlessCounterpartyService>(sp =>
        {
            var service = sp.GetRequiredService<CounterpartyService>();
            var logger = sp.GetRequiredService<ILogger<ExceptionlessCounterpartyService>>();
            return new ExceptionlessCounterpartyService(service, logger);
        });
        services.AddTransient<OrderService>();
        services.AddTransient<IOrderService, ExceptionlessOrderService>(sp =>
        {
            var service = sp.GetRequiredService<OrderService>();
            var logger = sp.GetRequiredService<ILogger<ExceptionlessOrderService>>();
            return new ExceptionlessOrderService(service, logger);
        });

        services.AddSingleton<IDialogService, DialogService>();

        return services;
    }
}
