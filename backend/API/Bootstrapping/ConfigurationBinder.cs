using Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace API.Bootstrapping
{
    public static class ConfigurationBinder
    {
        public static AppSettings Bind(IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppSettings>();
            configuration.Bind(settings);
            
            services.AddSingleton<IMongoDbSettings>(settings.MongoDb);
            services.AddSingleton<IJwtSettings>(settings.Jwt);
            services.AddSingleton<IEnvironmentSettings>(settings.Environment);

            return settings;
        }
    }
}