using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using Orders.Management.UI.ViewModels;
using System.Windows;

namespace Orders.Management.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost _host;
    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddViewModels();

        builder.Services.AddStorage(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? string.Empty);

        builder.Services.AddApplication();

        _host = builder.Build();

        var sessionFactory = _host.Services.GetRequiredService<ISessionFactory>();

        using (var session = sessionFactory.OpenSession()){ }

        await _host.StartAsync();

        MainWindow mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }
    }
}
