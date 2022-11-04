using Application.Domain;
using MongoDB.Driver;

namespace DataSeeder.DataSets.Utilities;

public class DataClearer
{
    private readonly MongoContext _context;

    public DataClearer(MongoContext context)
    {
        _context = context;
    }

    public async Task ClearAllData()
    {
        await _context.Threads.DeleteManyAsync(FilterDefinition<MoolThread>.Empty);
        await _context.Users.DeleteManyAsync(FilterDefinition<User>.Empty);
    }
}