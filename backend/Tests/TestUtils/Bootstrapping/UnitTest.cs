using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace Tests.TestUtils.Bootstrapping
{
    /// <summary>
    /// Base unit test class for closely testing objects.
    /// </summary>
    /// <typeparam name="T">Unit test class, used to create unique database.</typeparam>
    public abstract class UnitTest<T> : IAsyncLifetime
    {
        protected readonly IFixture Fixture = CustomFixtureCreator.Create();
        protected static readonly MongoAccessor Mongo = new(typeof(T).Name);
        
        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync() => await Mongo.DropDatabase();
    }
}