namespace EMS.FrontEnd {
    using System.Windows;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyModel;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using ViewModels;
    using Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private IHost host;
        private IServiceScope scope;
        protected override async void OnStartup(StartupEventArgs e) {
            host = CreateHostBuilder(e.Args).Build();
            await host.StartAsync();
            scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<MainWindow>().Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e) {
            await host.StopAsync();
            host.Dispose();
            scope.Dispose();
            base.OnExit(e);
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                    builder.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build())
                .ConfigureServices(services => {
                    services.AddTransient<MainWindow>().AddTransient<MainWindowViewModel>();
                    services.AddTransient<UserListView>().AddTransient<UserListViewModel>();
                })
                .UseSerilog((context, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration, DependencyContext.Default));
        }
    }
}
