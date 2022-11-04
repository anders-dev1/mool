using System.Threading.Tasks;
using Application.Services.Session;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Tests.Services.Session
{
    public class AuthenticatedTokensCreatorTests
    {
        private readonly Mock<IAccessTokenCreator> _accessTokenCreator = new Mock<IAccessTokenCreator>();
        private readonly Mock<IRefreshTokenCreator> _refreshTokenCreater = new Mock<IRefreshTokenCreator>();

        private readonly AuthenticatedTokensCreator _authenticatedTokensCreator;
        
        public AuthenticatedTokensCreatorTests()
        {
            _authenticatedTokensCreator = new AuthenticatedTokensCreator(
                _accessTokenCreator.Object,
                _refreshTokenCreater.Object);
        }
        
        [Fact]
        public async Task Create_CallsTokenCreatorsWithIdAndReturnsResult()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId().ToString();
            var accessToken = "legitAccessToken";
            var refreshToken = "legitRefreshToken";
            _accessTokenCreator.Setup(e => e.Create(It.Is<string>(id => id == userId))).Returns(accessToken);
            _refreshTokenCreater.Setup(e => e.Create(It.Is<string>(id => id == userId))).ReturnsAsync(refreshToken);
            
            // Act
            var result = await _authenticatedTokensCreator.Create(userId);

            // Assert
            Assert.Equal(accessToken, result.AccessToken);
            Assert.Equal(refreshToken, result.RefreshToken);
        }
    }
}