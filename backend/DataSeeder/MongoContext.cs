using Application.Domain;
using MongoDB.Driver;
using Shared;

namespace DataSeeder;

public class MongoContext
{
    public IMongoCollection<User> Users { get; }
    public IMongoCollection<MoolThread> Threads { get; }

    public MongoContext(IMongoDbSettings settings)
    {
        Users = Collection<User>(settings);
        Threads = Collection<MoolThread>(settings);
    }
    
    private static IMongoCollection<T> Collection<T>(IMongoDbSettings settings)
    {
        var database = settings!.GetDatabase();
        return database.GetCollection<T>(CollectionName<T>());
    }

    private static string CollectionName<T>()
    {
        return typeof(T).Name.ToLowerInvariant();
    }
}
