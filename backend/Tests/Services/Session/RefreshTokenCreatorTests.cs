using System;
using System.Threading.Tasks;
using Application.Services.Session;
using Application.Services.Session.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Services.Session
{
    public class RefreshTokenCreatorTests : UnitTest<RefreshTokenCreatorTests>, IAsyncLifetime
    {
        private readonly IMongoCollection<RefreshToken> _tokens = Mongo.GetCollection<RefreshToken>();

        private readonly RefreshTokenCreator _refreshTokenCreator;
        
        public RefreshTokenCreatorTests()
        {
            _refreshTokenCreator = new RefreshTokenCreator(_tokens);
        }

        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _tokens.DeleteManyAsync(FilterDefinition<RefreshToken>.Empty);
        }
        
        [Fact]
        public async Task Create_InsertsOneTokenWithGivenUserId()
        {
            // Arrange
            var id = ObjectId.GenerateNewId();

            // Act
            await _refreshTokenCreator.Create(id.ToString());

            // Assert
            var allTokens = await _tokens.Find(FilterDefinition<RefreshToken>.Empty).ToListAsync();
            Assert.Single(allTokens);
            Assert.Single(allTokens, e => e.UserId == id);
        }

        [Fact]
        public async Task Create_InsertsTokenWithOneYearExpiration()
        {
            // Arrange
            var id = ObjectId.GenerateNewId();

            // Act
            await _refreshTokenCreator.Create(id.ToString());
            
            // Assert
            var token = await _tokens.Find(e => e.UserId == id).SingleOrDefaultAsync();
            
            // Fuzzy assertion because token is generated before this code is run.
            var from = DateTimeOffset.Now.AddYears(1).AddMinutes(-1);
            var to = DateTimeOffset.Now.AddYears(1).AddMinutes(1);
            Assert.True(token.Expires > from && token.Expires < to);
        }
    }
}