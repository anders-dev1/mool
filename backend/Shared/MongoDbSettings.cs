#pragma warning disable 8618
namespace Shared
{
    // The NormalConnectionString field is used first if it is present otherwise the
    // MongoDbServerConnection will be used.
    public interface IMongoDbSettings
    {
        string? StandardConnectionString { get; set; }
        MongoDbSrvConnection? MongoDbSrvConnection { get; set; }
        public string DatabaseName { get; set; }
    }
    
    public class MongoDbSettings : IMongoDbSettings
    {
        public string? StandardConnectionString { get; set; }
        public MongoDbSrvConnection? MongoDbSrvConnection { get; set; }
        public string DatabaseName { get; set; }
    }

    public class MongoDbSrvConnection
    {
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}