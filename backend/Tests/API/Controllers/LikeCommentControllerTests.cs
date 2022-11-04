using System.Net;
using Application.Features;
using AutoFixture;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.API.Controllers
{
    public partial class ApiTests
    {
        [Fact]
        public async void Comment_Like_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);
            var command = Fixture.Create<LikeCommentCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/comment/{command.Id}/like", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void Comment_Like_WhenNotAuthorized_ReturnsUnauthorized()
        {
            var command = Fixture.Create<LikeCommentCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/comment/{command.Id}/like", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Comment_Unlike_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);
            var command = Fixture.Create<UnlikeCommentCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/comment/{command.Id}/unlike", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async void Comment_Unlike_WhenNotAuthorized_ReturnsUnauthorized()
        {
            var command = Fixture.Create<UnlikeCommentCommand>();
            
            // Act
            var response = await TestClient.PostAsync($"api/comment/{command.Id}/unlike", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}