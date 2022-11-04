using API.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Bootstrapping
{
    public static class SolutionBootstrapper
    {
        public static void Bootstrap(IServiceCollection services, IConfiguration configuration)
        {
            var settings = ConfigurationBinder.Bind(services, configuration);
            
            MediatRBootstrapper.Bootstrap(services);
            MongoCollectionBootstrapper.Bootstrap(services);
            ApplicationBootstrapper.Bootstrap(services);
            
            AuthenticationBootstrapper.Bootstrap(services, settings);
            
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidationExceptionFilter));
                options.Filters.Add(typeof(UnauthorizedExceptionFilter));
                options.Filters.Add(typeof(NotFoundExceptionFilter));
                options.Filters.Add(typeof(BadRequestExceptionFilter));
                options.Filters.Add(typeof(ConflictExceptionFilter));
            }).AddNewtonsoftJson();
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyHeader();
                        builder.AllowAnyOrigin();
                    });
            });

            services.AddSwaggerDocument();
        }
    }
}