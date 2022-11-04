using Application.Domain;
using Application.Exceptions;
using Application.Services.Session;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Services.Session
{
    public class ContextUserFetcherTests : UnitTest<ContextUserFetcherTests>
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new Mock<IHttpContextAccessor>();
        
        private readonly ContextUserFetcher _contextUserFetcher;

        public ContextUserFetcherTests()
        {
            _contextUserFetcher = new ContextUserFetcher(_httpContextAccessor.Object);
        }

        private User ArrangeUserContext()
        {
            var user = Fixture.Create<User>();
            var httpContext = new DefaultHttpContext {Items = {[ContextUserFetcher.HttpContextUserKey] = user}};
            _httpContextAccessor.Setup(e => e.HttpContext).Returns(httpContext);

            return user;
        }

        private void ArrangeEmptyContext()
        {
            var httpContext = new DefaultHttpContext();
            _httpContextAccessor.Setup(e => e.HttpContext).Returns(httpContext);
        }
        
        [Fact]
        public void FetchOrThrow_WhenUserIsInContext_ReturnsUser()
        {
            // Arrange
            var seededUser = ArrangeUserContext();

            // Act
            var user = _contextUserFetcher.FetchOrThrow();

            // Assert
            Assert.Equal(seededUser, user);
        }
        
        [Fact]
        public void FetchOrThrow_WhenUserIsNotInContext_ThrowsUnuahtorizedException()
        {
            // Arrange
            ArrangeEmptyContext();
            
            // Act + Assert
            Assert.Throws<UnauthorizedException>(() => _contextUserFetcher.FetchOrThrow());
        }
        
        [Fact]
        public void Fetch_WhenUserIsInContext_ReturnsUser()
        {
            // Arrange
            var seededUser = ArrangeUserContext();

            // Act
            var user = _contextUserFetcher.Fetch();

            // Assert
            Assert.Equal(seededUser, user);
        }
        
        [Fact]
        public void Fetch_WhenUserIsNotInContext_ReturnsNull()
        {
            // Arrange
            ArrangeEmptyContext();

            // Act
            var user = _contextUserFetcher.Fetch();

            // Assert
            Assert.Null(user);
        }
    }
}