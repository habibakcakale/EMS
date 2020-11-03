namespace EMS.FrontEnd {
    using System.Net.Cache;
    using System.Text.Json;
    using System.Windows;
    using Integration;
    using Integration.User;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyModel;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RestSharp;
    using RestSharp.Authenticators;
    using RestSharp.Serializers.SystemTextJson;
    using Serilog;
    using Services;
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
                .ConfigureServices((context, services) => {
                    RegisterRestClient(context.Configuration, services);
                    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                    services.AddMediatR(typeof(GetUserList).Assembly);
                    services.AddTransient<ICsvExportService, ExportService>();
                    services.AddTransient<MainWindow>().AddTransient<MainWindowViewModel>();
                    services.AddTransient<UserListView>().AddTransient<UserListViewModel>();

                })
                .UseSerilog((context, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration, DependencyContext.Default));
        }

        private static void RegisterRestClient(IConfiguration configuration, IServiceCollection services) {
            var authToken = configuration.GetValue<string>("GoRest:AuthToken");
            var endPoint = configuration.GetValue<string>("GoRest:Endpoint");
            services.AddScoped<IRestClient>(provider => {
                var client = new RestClient(endPoint) {
                    Authenticator = new JwtAuthenticator(authToken),
                    // Cache policy should handle E-Tag request response caching 
                    CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default)
                }.UseSystemTextJson(new JsonSerializerOptions {IgnoreNullValues = true});
                return client;
            });
        }
    }
}
