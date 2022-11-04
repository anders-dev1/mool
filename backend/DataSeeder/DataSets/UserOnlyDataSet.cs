using Application.Domain;
using DataSeeder.DataSets.Utilities;

namespace DataSeeder.DataSets;

public class UserOnlyDataSet : IDataSet
{
    public string Id => "07e63583-63b4-451b-9760-5d953a24c86e";

    private readonly UserToSeed _user = new UserToSeed("test", "user", $"testemail@{nameof(UserOnlyDataSet)}.com", "!TestUser1");
    
    public async Task Seed(MongoContext context)
    {
        await DataSetHelper.SeedUser(_user, context);
    }

    public async Task Delete(MongoContext context)
    {
        await DataSetHelper.DeleteUserAndEverythingTheyHaveCreated(_user.Email, context);
    }
}