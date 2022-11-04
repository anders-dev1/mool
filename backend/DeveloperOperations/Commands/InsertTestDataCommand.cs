using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Domain;
using CliFx.Attributes;
using CliFx.Infrastructure;
using MongoDB.Driver;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace DeveloperOperations.Commands
{
    /// <summary>
    /// Inserts testdata into the environment specified in command args.
    /// </summary>
    [Command("insert-test-data")]
    public class InsertTestDataCommand : BaseCommand
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<MoolThread> _threads;
        
        public InsertTestDataCommand(
            IMongoCollection<User> users,
            IMongoCollection<MoolThread> threads)
        {
            _users = users;
            _threads = threads;
        }

        public override async ValueTask ExecuteAsync(IConsole console)
        {
            var randomizerFirstName = RandomizerFactory.GetRandomizer(new FieldOptionsFirstName());
            var randomizerLastName = RandomizerFactory.GetRandomizer(new FieldOptionsLastName());
            var randomizerEmail = RandomizerFactory.GetRandomizer(new FieldOptionsEmailAddress());
            
            var users = Enumerable.Repeat<object>(null, 10).Select(e => new User(
                randomizerFirstName.Generate(), 
                randomizerLastName.Generate(), 
                randomizerEmail.Generate(), 
                Guid.NewGuid().ToString()))
                .ToList();
            await _users.InsertManyAsync(users);

            var randomizerLorem = RandomizerFactory.GetRandomizer(new FieldOptionsTextLipsum());
            var random = new Random();
            var threads = Enumerable.Repeat<object>(null, 50).Select(e =>
                new MoolThread(
                    randomizerLorem.Generate(), 
                    users[random.Next(0, users.Count() - 1)].Id)
            );
            await _threads.InsertManyAsync(threads);
        }
    }
}