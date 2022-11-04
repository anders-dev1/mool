using Application.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DataSeeder.DataSets.Utilities;

public record UserToSeed(string FirstName, string LastName, string Email, string Password);

public class DataSetHelper
{
    public static async Task<ObjectId> SeedUser(UserToSeed userToSeed, MongoContext context)
    {
        var user = await context.Users.AsQueryable().FirstOrDefaultAsync(e => e.Email == userToSeed.Email);
        if (user != null)
        {
            throw new Exception("Data has already been seeded!");
        }
        
        // Map to document
        var userDocument = new User(
            userToSeed.FirstName, 
            userToSeed.LastName, 
            userToSeed.Email, 
            BCrypt.Net.BCrypt.HashPassword(userToSeed.Password));

        await context.Users.InsertOneAsync(userDocument);
        return userDocument.Id;
    }

    public static async Task DeleteUserAndEverythingTheyHaveCreated(string email, MongoContext context)
    {
        // get user
        var user = await context.Users.AsQueryable().FirstOrDefaultAsync(e => e.Email == email);
        if (user == null)
        {
            throw new Exception("Couldn't find user to delete");
        }
        
        // delete all comment likes.
        await context.Threads.UpdateManyAsync(
            e => e.Comments.Any(i => i.LikedBy.Contains(user.Id)),
            Builders<MoolThread>.Update.Pull(e => e.Comments[-1].LikedBy, user.Id));

        // delete all comments.
        var pull = Builders<MoolThread>.Update.PullFilter(e =>
            e.Comments, comment => comment.UserId == user.Id);
        await context.Threads.UpdateManyAsync(FilterDefinition<MoolThread>.Empty, pull);

        // delete all thread likes.
        await context.Threads.UpdateManyAsync(e => e.LikedBy.Any(i => i == user.Id),
            Builders<MoolThread>.Update.Pull(e => e.LikedBy, user.Id));

        // delete all threads.
        var filter = new FilterDefinitionBuilder<MoolThread>();
        var filter2 = filter.Eq(e => e.User, user.Id);
        await context.Threads.DeleteManyAsync(filter2);
        
        // delete user
        await context.Users.DeleteOneAsync(e => e.Id == user.Id);
    }
}