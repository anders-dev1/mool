using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features;
using Application.Services.Session;
using Application.Services.Session.Model;
using AutoFixture;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class RenewAccessTokenCommandHandlerTests : UnitTest<RenewAccessTokenCommandHandlerTests>, IAsyncLifetime
    {
        private readonly IMongoCollection<RefreshToken> _refreshTokenCollection = Mongo.GetCollection<RefreshToken>();

        private readonly Mock<IAccessTokenCreator> _accessTokenCreator = new Mock<IAccessTokenCreator>();
        private readonly RenewAccessTokenCommandHandler _renewAccessTokenCommandHandler;
        
        public RenewAccessTokenCommandHandlerTests()
        {
            _renewAccessTokenCommandHandler = new RenewAccessTokenCommandHandler(
                _refreshTokenCollection,
                _accessTokenCreator.Object);
        }
        
        public async Task InitializeAsync() => await DisposeAsync(); 
        public async Task DisposeAsync()
        {
            await _refreshTokenCollection.DeleteManyAsync(FilterDefinition<RefreshToken>.Empty);
        }

        [Fact]
        public async Task Handle_WhenRequestIdIsValidAndRefreshTokenIsFound_ReturnsAccessTokenFromCreator()
        {
            // Arrange
            var refreshTokenId = ObjectId.GenerateNewId();
            var refreshToken = Fixture.Build<RefreshToken>()
                .With(e => e.Id, refreshTokenId)
                .With(e => e.Expires, DateTimeOffset.Now.AddYears(1))
                .Create();
            await _refreshTokenCollection.InsertOneAsync(refreshToken);

            var expectedAccessToken = "legit_accesstoken";
            _accessTokenCreator.Setup(e => e.Create(It.Is<string>(e => e == refreshToken.UserId.ToString()))).Returns(expectedAccessToken);

            var request = new RenewAccessTokenCommand(refreshTokenId.ToString());
            
            // Act
            var result = await _renewAccessTokenCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(expectedAccessToken, result.AccessToken);
        }
        
        [Fact]
        public async Task Handle_WhenRequestIdIsInvalid_ThrowsBadRequest()
        {
            // Arrange
            var request = new RenewAccessTokenCommand("invalid because not objectId");

            // Act + Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _renewAccessTokenCommandHandler.Handle(request, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_WhenRefreshTokenIsNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new RenewAccessTokenCommand(ObjectId.GenerateNewId().ToString());

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _renewAccessTokenCommandHandler.Handle(request, CancellationToken.None));
        }
    }
}