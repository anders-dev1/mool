using Application.Domain;
using DataSeeder.DataSets.Utilities;
using MongoDB.Driver;

namespace DataSeeder.DataSets;

public class UserWithCreatedThreadWithCreatedComment : IDataSet
{
    public string Id => "bf27df9c-f97d-4ffe-acc0-221be5efa975";
    
    private static string stringId = nameof(UserWithCreatedThreadWithCreatedComment);
    
    private readonly UserToSeed _interactingUser = new UserToSeed("test1", "user1", $"testemail1@{stringId}.com", "!TestUser1");
    private readonly UserToSeed _userThatCreatedThread = new UserToSeed("test2", "user2", $"testemail2@{stringId}.com", "!TestUser1");
    private readonly UserToSeed _userThatCreatedComment = new UserToSeed("test3", "user3", $"testemail3@{stringId}.com", "!TestUser1");

    public async Task Seed(MongoContext context)
    {
        await DataSetHelper.SeedUser(_interactingUser, context);
        var userThatCreatedThreadId = await DataSetHelper.SeedUser(_userThatCreatedThread, context);
        await DataSetHelper.SeedUser(_userThatCreatedComment, context);
        
        var thread = new MoolThread(
            $"Hey, this is the content for test set {stringId}",
            userThatCreatedThreadId);
        await context.Threads.InsertOneAsync(thread);

        var comment = new Comment(
            $"this is the comment content for test set: {stringId}", 
            userThatCreatedThreadId,
            DateTimeOffset.UtcNow);
        await context.Threads.UpdateOneAsync(
            e => e.Id == thread.Id, 
            Builders<MoolThread>.Update.AddToSet(e => e.Comments, comment));
    }

    public Task Delete(MongoContext context)
    {
        return Task.CompletedTask;
    }
}