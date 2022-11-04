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
        public async void CommentController_Comments_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);

            // Act
            var response = await TestClient.GetAsync("api/thread/ThreadId/comments");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Fact]
        public async void CommentController_Comments_WhenNotAuthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await TestClient.GetAsync("api/thread/ThreadId/comments");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async void CommentController_Create_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);
            var content = "test";
            
            // Act
            var response = await TestClient.PostAsync($"api/thread/5/comments", content.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async void CommentController_Create_WhenNotAuthorized_ReturnsUnauthorized()
        {
            // Arrange
            var command = Fixture.Create<CommentCreationCommand>();

            // Act
            var response = await TestClient.PostAsync($"api/thread/{command.ThreadId}/comments", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}