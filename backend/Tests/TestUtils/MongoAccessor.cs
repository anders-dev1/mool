using System;
using System.Threading.Tasks;
using API.Bootstrapping;
using MongoDB.Driver;

namespace Tests.TestUtils;

public class MongoAccessor
{
    private readonly MongoClient _mongoClient;
    private readonly string _database;

    public MongoAccessor(string database)
    {
        _mongoClient = CreateMongoClient();
        _database = database;
    }

    private MongoClient CreateMongoClient()
    {
        var settings = TestAppSettingsGetter.Get();

        var localServerSettings = 
            MongoClientSettings.FromConnectionString(settings.MongoDb.StandardConnectionString);

        var localServerConnectionTimeout = TimeSpan.FromSeconds(1);
        localServerSettings.ConnectTimeout = localServerConnectionTimeout;
        localServerSettings.ServerSelectionTimeout = localServerConnectionTimeout;
        localServerSettings.SocketTimeout = localServerConnectionTimeout;
                
        var client = new MongoClient(localServerSettings);

        return client;
    }
    
    public IMongoCollection<T> GetCollection<T>()
    {
        return _mongoClient.GetDatabase(_database).GetCollection<T>(
            MongoCollectionBootstrapper.CollectionName<T>());
    }

    public async Task DropDatabase()
    {
        await _mongoClient.DropDatabaseAsync(_database);
    }
}