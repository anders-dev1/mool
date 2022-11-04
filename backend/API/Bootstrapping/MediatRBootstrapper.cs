using Application.PipelineBehaviour;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace API.Bootstrapping
{
    public static class MediatRBootstrapper
    {
        public static void Bootstrap(IServiceCollection services)
        {
            var mediatrAssembly = typeof(ValidationBehaviour<,>).Assembly;
            services.AddMediatR(mediatrAssembly);

            // Validation pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddValidatorsFromAssembly(typeof(ValidationBehaviour<,>).Assembly);
        }
    }
}