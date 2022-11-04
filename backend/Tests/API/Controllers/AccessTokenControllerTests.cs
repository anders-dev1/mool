using System;
using System.Net;
using System.Threading;
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
        public async void Authenticate_EndpointIsConfiguredCorrectly()
        {
            // Arrange
            var authenticationQuery = Fixture.Create<AuthenticationQuery>();
            var authenticationResult = Fixture.Create<AuthenticationResult>();
            Mediator.Setup(e => 
                e.Send(authenticationQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authenticationResult);
            
            // Act
            var response = await TestClient.PostAsync(
                "api/accesstoken/authenticate",
                authenticationQuery.ToJson());
            var typeResponse = await response.ReadAsType<AuthenticationResult>();
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(authenticationResult, typeResponse);
        }
        
        [Fact]
        public async void Renew_EndpointIsConfiguredCorrectly()
        {
            // Arrange
            var renewAccessTokenCommand = Fixture.Create<RenewAccessTokenCommand>();
            var renewAccessTokenResult = Fixture.Create<RenewAccessTokenResult>();
            Mediator.Setup(e => 
                    e.Send(renewAccessTokenCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(renewAccessTokenResult);
            
            // Act
            var response = await TestClient.PostAsync(
                "api/accesstoken/renew",
                renewAccessTokenCommand.ToJson());
            var typeResponse = await response.ReadAsType<RenewAccessTokenResult>();
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(renewAccessTokenResult, typeResponse);
        }
    }
}