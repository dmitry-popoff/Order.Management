using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Counterparties.Queries;
using Orders.Management.Domain.Entities.Employees;
using Orders.Management.Domain.Entities.Employees.Queries;
using Orders.Management.Domain.Entities.Orders;
using Orders.Management.Domain.Entities.Orders.Queries;
using Orders.Management.Persistence.Mapping.Entities.Counterparties;
using Orders.Management.Persistence.Mapping.Entities.Employees;
using Orders.Management.Persistence.Mapping.Entities.Employees.Queries;
using Orders.Management.Persistence.Mapping.Entities.Orders;
using Orders.Management.Persistence.Mapping.Entities.Orders.Queries;
using Shared.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddEmployeeMappings();
        services.AddCounterpartyMappings();
        services.AddOrderMappings();

        return services;
    }

    internal static IServiceCollection AddEmployeeMappings(this IServiceCollection services)
    {
        services.AddTransient<IQueryExecutor<GetEmployeesByNameQuery, PagedResult<EmployeeDetails>>, GetEmployeeQueryExecutor>();
        services.AddTransient<IFinder<Employee>, EmployeeFinder>();
        services.AddTransient<IStorage<Employee>, EmployeeStorage>();
        services.AddTransient<IQueryObject<EmployeeData>, EmployeeDataQueryObject>();

        return services;
    }
    
    internal static IServiceCollection AddCounterpartyMappings(this IServiceCollection services)
    {
        services.AddTransient<IQueryExecutor<GetCounterpartiesQuery, PagedResult<CounterpartyList>>, GetCounterpartyQueryExecutor>();
        services.AddTransient<IFinder<Counterparty>, CounterpartyFinder>();
        services.AddTransient<IStorage<Counterparty>, CounterpartyStorage>();
        services.AddTransient<IQueryObject<CounterpartyData>, CounterpartyQueryObject>();

        return services;
    }

    internal static IServiceCollection AddOrderMappings(this IServiceCollection services)
    {
        services.AddTransient<IQueryExecutor<GetOrdersFilter, PagedResult<OrderList>>, GetOrdersQueryExecutor>();
        services.AddTransient<IFinder<Order>, OrderFinder>();
        services.AddTransient<IStorage<Order>, OrderStorage>();
        services.AddTransient<IQueryObject<OrderData>, OrderDataQueryObject>();
        
        return services;
    }
}
