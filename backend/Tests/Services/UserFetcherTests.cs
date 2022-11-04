using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Domain;
using Application.Services;
using AutoFixture;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Services
{
    public class UserFetcherTests : UnitTest<UserFetcherTests>, IAsyncLifetime
    {
        private readonly IMongoCollection<User> _userCollection = Mongo.GetCollection<User>();
        
        private readonly UserFetcher _userFetcher;

        public UserFetcherTests()
        {
            _userFetcher = new UserFetcher(_userCollection);
        }

        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _userCollection.DeleteManyAsync(FilterDefinition<User>.Empty);
        }

        [Fact]
        public async Task Fetch_WhenGivenIdOfExistingUser_ReturnsQueriedUser()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();
            var user = Fixture.Build<User>()
                .With(e => e.Id, userId)
                .Create();
            await _userCollection.InsertOneAsync(user);

            // Act
            var result = await _userFetcher.Fetch(userId);

            // Assert
            Assert.Equal(user, result);
        }
        
        [Fact]
        public async Task Fetch_WhenGivenIdOfNonExistingUser_ReturnsNull()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();

            // Act
            var result = await _userFetcher.Fetch(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Fetch_WhenGivenIdsOfExistingUsers_ReturnsQueriedUsers()
        {
            // Arrange
            var users = Fixture.CreateMany<User>(3).ToList();
            await _userCollection.InsertManyAsync(users);

            // Act
            var result = await _userFetcher.Fetch(users.Select(e => e.Id));
            var resultUsers = result.ToList();

            // Assert
            foreach (var user in users)
            {
                Assert.Contains(user, resultUsers);
            }
        }
        
        [Fact]
        public async Task Fetch_WhenGivenIdOfNonExistingUsers_ReturnsEmptyCollection()
        {
            // Arrange
            var users = Fixture.CreateMany<User>(3).ToList();
            await _userCollection.InsertManyAsync(users);

            var ids = Enumerable.Repeat(ObjectId.GenerateNewId(), 5).ToList();

            // Act
            var result = await _userFetcher.Fetch(ids);

            // Assert
            Assert.Empty(result);
        }
    }
}