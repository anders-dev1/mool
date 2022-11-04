using Application.PipelineBehaviour;
using Application.Services;
using Application.Services.Session;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace API.Bootstrapping
{
    public static class ApplicationBootstrapper
    {
        public static void Bootstrap(IServiceCollection services)
        {
            // General
            services.AddSingleton<IPasswordIssuer, PasswordIssuer>();
            services.AddSingleton<IPasswordIssuer, PasswordIssuer>();
            services.AddSingleton<IAccessTokenCreator, AccessTokenCreator>();
            services.AddSingleton<IRefreshTokenCreator, RefreshTokenCreator>();
            services.AddSingleton<IAuthenticatedTokensCreator, AuthenticatedTokensCreator>();
            services.AddSingleton<IUtcNowGetter, UtcNowGetter>();

            // User-related
            services.AddSingleton<IUserFetcher, UserFetcher>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IContextUserFetcher, ContextUserFetcher>();
        }
    }
}