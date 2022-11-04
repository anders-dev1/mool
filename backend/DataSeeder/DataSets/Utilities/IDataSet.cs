namespace DataSeeder.DataSets.Utilities;

public interface IDataSet
{
    string Id { get; }
    Task Seed(MongoContext context);
}