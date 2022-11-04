using DataSeeder.DataSets.Utilities;

namespace DataSeeder.DataSets;

public class EmptySet : IDataSet
{
    public string Id => "d666ea00-b1be-4db6-8a9a-7911d7465b21";
    
    public Task Seed(MongoContext context)
    {
        return Task.CompletedTask;
    }

    public Task Delete(MongoContext context)
    {
        return Task.CompletedTask;
    }
}