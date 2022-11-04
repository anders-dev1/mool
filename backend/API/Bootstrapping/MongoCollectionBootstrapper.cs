using System;
using Application.Domain;
using Application.Services.Session.Model;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared;

namespace API.Bootstrapping
{
    public static class MongoCollectionBootstrapper
    {
        public static void Bootstrap(IServiceCollection services)
        {
            services.AddSingleton(Collection<User>());
            services.AddSingleton(Collection<MoolThread>());
            services.AddSingleton(Collection<RefreshToken>());
        }

        private static Func<IServiceProvider, IMongoCollection<T>> Collection<T>()
        {
            return (provider) =>
            {
                var settings = provider.GetService<IMongoDbSettings>();

                var database = settings!.GetDatabase();
                
                return database.GetCollection<T>(CollectionName<T>());
            };
        }

        public static string CollectionName<T>()
        {
            return typeof(T).Name.ToLowerInvariant();
        }
    }
}