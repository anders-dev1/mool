using System.Text;
using MongoDB.Driver;

namespace Shared
{
    public static class IMongoDbSettingsExtensions
    {
        public static IMongoDatabase GetDatabase(this IMongoDbSettings settings)
        {
            var client = CreateClient(settings);
            return client.GetDatabase(settings.DatabaseName);
        }

        private static IMongoClient CreateClient(this IMongoDbSettings settings)
        {
            if (settings.StandardConnectionString == null && settings.MongoDbSrvConnection == null)
            {
                throw new Exception("No database connection defined in config.");
            }
            
            if (settings.StandardConnectionString != null)
            {
                return new MongoClient(settings.StandardConnectionString);
            }
            
            return CreateSrvClient(settings);
        }

        // Couldn't figure out how to do it with MongoUrlBuilder so here we are :)
        private static IMongoClient CreateSrvClient(this IMongoDbSettings settings)
        {
            var srvAddress = settings.MongoDbSrvConnection!.Address;
            var afterSrvIndex = "mongodb+srv://".Length;

            var srvUsername = settings.MongoDbSrvConnection.Username;
            var srvPassword = settings.MongoDbSrvConnection.Password;
            
            var sb = new StringBuilder();
            sb.Append(srvAddress.Substring(0, afterSrvIndex));
            sb.Append($"{srvUsername}:{srvPassword}@");
            sb.Append(srvAddress.Substring(afterSrvIndex, srvAddress.Length - afterSrvIndex));

            var srvConnectionString = sb.ToString();
            return new MongoClient(srvConnectionString);
        }
    }
}