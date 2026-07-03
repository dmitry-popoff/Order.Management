using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver.MySqlConnector;
using NHibernate.Tool.hbm2ddl;
using Orders.Management.Persistence.Mapping;
using System.Reflection.Metadata;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string connectionString)
    {
        services.AddSessionFactory(connectionString);
        services.AddMappings();

        return services;
    }

    internal static IServiceCollection AddSessionFactory(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<ISessionFactory>(sp =>
             Fluently.Configure()
                       .Database(
                            MySQLConfiguration.Standard
                                .Dialect<MySQL8Dialect>()
                                .Driver<MySqlConnectorDriver>()
                                .ConnectionString(connectionString)                                
                           )
                       .Mappings(m => m.FluentMappings.AddFromAssembly(MappingAssembly.Assembly))
                       //.ExposeConfiguration(cfg => BuildSchema(cfg))
                       .BuildSessionFactory()
                       );

        return services;
    }

    private static void BuildSchema(Configuration cfg)
    {
        try
        {
            using TextWriter textWriter = new StringWriter();
            new SchemaExport(cfg).Execute(
                useStdOut: true,         // Prints script to the console
                execute: true,          // Set to true if you want to run it against the DB right now
                justDrop: false,         // Set to true to generate drop commands only
                connection: null,        // Pass an active connection if executing
                textWriter
            );

            File.WriteAllText("create_script.sql", textWriter.ToString());
        }
        catch { }
    }
}
