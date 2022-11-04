using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Features;
using Application.Services;
using Application.Services.Session;
using AutoFixture;
using MongoDB.Driver;
using Moq;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class AuthenticationQueryHandlerTests : UnitTest<AuthenticationQueryHandlerTests>, IAsyncLifetime
    {
        // Collections
        private readonly IMongoCollection<User> _userCollection = Mongo.GetCollection<User>();

        // Services
        private readonly Mock<IPasswordIssuer> _passwordMatchVerifier = new Mock<IPasswordIssuer>();
        private readonly Mock<IAuthenticatedTokensCreator> _authenticatedTokensCreator = new Mock<IAuthenticatedTokensCreator>();

        // sutre
        private readonly AuthenticationQueryHandler _authenticationQueryHandler;

        public AuthenticationQueryHandlerTests()
        {
            _authenticationQueryHandler = new AuthenticationQueryHandler(
                _userCollection,
                _passwordMatchVerifier.Object,
                _authenticatedTokensCreator.Object);
        }

        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _userCollection.DeleteManyAsync(FilterDefinition<User>.Empty);
        }

        private async Task<(User user, AuthenticationQuery query)> SeedUser()
        {
            var user = Fixture.Create<User>();
            await _userCollection.InsertOneAsync(user);
            
            var query = Fixture.Create<AuthenticationQuery>() with
            {
                Email = user.Email
            };

            return (user, query);
        }

        [Fact]
        public async Task Handle_WhenEverythingIsOk_ReturnsToken()
        {
            // Arrange
            _passwordMatchVerifier.Setup(e => e.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            
            var tokens = Fixture.Create<AuthenticatedTokens>();
            _authenticatedTokensCreator.Setup(e => e.Create(It.IsAny<string>()))
                .ReturnsAsync(tokens);

            var (_, query) = await SeedUser();

            // Act
            var result = await _authenticationQueryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(tokens, result.Tokens);
        }
        
        [Fact]
        public async Task Handle_RequestedUserIsNotFound_ThrowsNotFound()
        {
            // Arrange
            var query = Fixture.Create<AuthenticationQuery>();
            
            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _authenticationQueryHandler.Handle(query, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_WhenPassworDoesntMatcherUserPassword_ThrowsUnauthorized()
        {
            // Arrange
            var (_, query) = await SeedUser();
            
            _passwordMatchVerifier.Setup(e => e.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
            
            // Act + Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _authenticationQueryHandler.Handle(query, CancellationToken.None));
        }
    }
}