using Microsoft.Extensions.DependencyInjection;
using SpeedrunCommunity.Persistence;
using SpeedrunCommunity.ViewModels.Pages;
using System;
using System.Windows;

namespace SpeedrunCommunity;

public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        ServiceCollection services = new();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Database
        services.AddDbContext<SRCContext>(ServiceLifetime.Transient);
        
        // ViewModels
        services.AddTransient<PlayerViewModel>();
        services.AddTransient<GameViewModel>();
        services.AddTransient<ResultViewModel>();
        
        // Main Window (needs IServiceProvider for navigation)
        services.AddTransient<MainWindow>(sp => new MainWindow(sp));
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using (var scope = ServiceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<SRCContext>();
            DbInitializer.Initialize(context);
        }

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
