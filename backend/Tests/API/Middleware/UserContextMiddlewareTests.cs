using System;
using Application.Services;
using Application.Services.Session;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.API.Middleware
{
    public class UserContextMiddlewareTests : ApiTest<UserContextMiddlewareTests>
    {
        [Fact]
        public async void WhenTokenIsVerifiedUser_AddsUserToContext()
        {
            // Arrange
            var mock = ApiUserMocker.Seed(this);
            
            // Act
            var context = await Server.SendAsync(e =>
            {
                e.Request.Headers.Add("Authorization", $"Bearer {mock.Token}");
            });
            
            // Assert
            var user = context.Items[ContextUserFetcher.HttpContextUserKey];
            Assert.Equal(mock.User, user);
        }

        [Fact]
        public async void WhenTokenIsNotIncluded_ContextIsNotAuthenticated()
        {
            // Act
            var context = await Server.SendAsync(e => { });
            
            // Assert
            Assert.False(context.User.Identity!.IsAuthenticated);
            Assert.Null(context.Items[ContextUserFetcher.HttpContextUserKey]);
        }
    }
}