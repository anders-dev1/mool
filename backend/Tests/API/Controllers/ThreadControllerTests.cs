using System;
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
        public async void Create_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);
            var command = Fixture.Create<ThreadCreationCommand>();
            
            // Act
            var response = await TestClient.PostAsync("api/thread", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async void Create_WhenNotAuthorized_ReturnsUnauthorized()
        {
            // Arrange
            var command = Fixture.Create<ThreadCreationCommand>();
            
            // Act
            var response = await TestClient.PostAsync("api/thread", command.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async void List_WhenAuthorized_ReturnsOk()
        {
            // Arrange
            ApiUserMocker.Seed(this);
            
            // Act
            var response = await TestClient.GetAsync("api/thread");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Fact]
        public async void List_WhenNotAuthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await TestClient.GetAsync("api/thread");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}