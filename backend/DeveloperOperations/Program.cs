using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using API;
using API.Bootstrapping;
using CliFx;
using DeveloperOperations.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperOperations
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var services = BootstrapSolution(args);
            BootstrapCommands(services);
            
            var serviceProvider = services.BuildServiceProvider();
            return await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(serviceProvider.GetService)
                .Build()
                .RunAsync();
        }

        private static ServiceCollection BootstrapSolution(string[] args)
        {
            var configString = EnvironmentArgumentHelper.GetConfigStringFromArgs(args);
            
            var apiAssembly = Assembly.GetAssembly(typeof(Startup));
            var apiLocation = Path.GetDirectoryName(apiAssembly!.Location);
            
            var services = new ServiceCollection();
            var cb = new ConfigurationBuilder()
                .SetBasePath(apiLocation)
                .AddJsonFile("appsettings.json");
            cb.AddJsonFile($"appsettings.{configString}.json");
            
            var configuration = cb.Build();

            SolutionBootstrapper.Bootstrap(services, configuration);

            return services;
        }

        private static void BootstrapCommands(ServiceCollection services)
        {
            services.AddTransient<InsertTestDataCommand>();
        }
    }
}