using System.Net;
using System.Threading;
using Application.Features;
using AutoFixture;
using Moq;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.API.Controllers
{
    public partial class ApiTests
    {
        [Fact]
        public async void Like_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);
            var command = Fixture.Create<LikeThreadCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/thread/{command.Id}/like", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void Like_WhenNotAuthorized_ReturnsUnauthorized()
        {
            var command = Fixture.Create<LikeThreadCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/thread/{command.Id}/like", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Unlike_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);
            var command = Fixture.Create<UnlikeThreadCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/thread/{command.Id}/unlike", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async void Unlike_WhenNotAuthorized_ReturnsUnauthorized()
        {
            var command = Fixture.Create<UnlikeThreadCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/thread/{command.Id}/unlike", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}