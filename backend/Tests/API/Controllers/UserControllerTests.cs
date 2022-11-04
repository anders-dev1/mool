using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Features;
using AutoFixture;
using Moq;
using Tests.TestUtils;
using Xunit;

namespace Tests.API.Controllers
{
    public partial class ApiTests
    {
        [Fact]
        public async Task Create_HasExpectedRouteAndPublishesCommand()
        {
            // Arrange
            var command = Fixture.Create<UserCreationCommand>();

            // Act
            var response = await TestClient.PostAsync("api/user", command.ToJson());
        
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Mediator.Verify(e => e.Send(command, It.IsAny<CancellationToken>()));
        }
    }
}