namespace EMS.FrontEnd {
    using System.Windows;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyModel;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private IHost host;

        protected override async void OnStartup(StartupEventArgs e) {
            host = CreateHostBuilder(e.Args).Build();
            await host.StartAsync();
            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<MainWindow>().Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e) {
            await host.StopAsync();
            host.Dispose();
            base.OnExit(e);
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                    builder.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build())
                .ConfigureServices(services => {
                    services.AddScoped<MainWindow>();
                    services.AddTransient<MainWindowViewModel>();
                    services.AddHttpClient();
                })
                .UseSerilog((context, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration, DependencyContext.Default));
        }
    }
}
