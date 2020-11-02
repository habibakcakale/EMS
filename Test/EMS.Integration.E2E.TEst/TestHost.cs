namespace EMS.Integration.E2E.Test {
    using System;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RestSharp;
    using RestSharp.Authenticators;
    using RestSharp.Serializers.SystemTextJson;

    public class TestHost : IDisposable {
        public IHost Host { get; set; } = CreateHostBuilder().Build();

        public static IHostBuilder CreateHostBuilder(string[] args = null) {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build())
                .ConfigureServices((context, services) => {
                   var authToken = context.Configuration.GetValue<string>("GoRest:AuthToken");
                   services.AddTransient<IRestClient>(provider => new RestClient("https://gorest.co.in/public-api/") {
                       Authenticator = new JwtAuthenticator(authToken)
                   }.UseJson());
                   services.AddMediatR(typeof(UserList).Assembly);
                });
        }

        public void Dispose() {
            Host.StopAsync();
            Host.Dispose();
        }
    }
}