namespace EMS.Integration.E2E.Test {
    using System;
    using System.Net.Cache;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RestSharp;
    using RestSharp.Authenticators;
    using System.Text.Json;
    using Integration.User;
    using RestSharp.Serializers.SystemTextJson;

    public class TestHost : IDisposable {
        public IHost Host { get; set; } = CreateHostBuilder().Build();

        public static IHostBuilder CreateHostBuilder(string[] args = null) {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build())
                .ConfigureServices((context, services) => {
                    RegisterRestClient(context, services);
                    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                    services.AddMediatR(typeof(GetUserList).Assembly);
                });
        }

       private static void RegisterRestClient(HostBuilderContext context, IServiceCollection services) {
            var authToken = context.Configuration.GetValue<string>("GoRest:AuthToken");
            var endPoint = context.Configuration.GetValue<string>("GoRest:Endpoint");
            services.AddScoped<IRestClient>(provider => {
                var client = new RestClient(endPoint) {
                    Authenticator = new JwtAuthenticator(authToken),
                    // Cache policy should handle E-Tag request response caching 
                    CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default)
                }.UseSystemTextJson(new JsonSerializerOptions {IgnoreNullValues = true});
                return client;
            });
        }

        public void Dispose() {
            Host.StopAsync();
            Host.Dispose();
        }
    }
}