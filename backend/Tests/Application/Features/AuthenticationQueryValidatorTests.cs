using System.Linq;
using Application.Features;
using AutoFixture;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class AuthenticationQueryValidatorTests : UnitTest<AuthenticationQueryValidatorTests>
    {
        private readonly AuthenticationQueryValidator _authenticationQueryValidator = new AuthenticationQueryValidator();

        [Fact]
        public void Validate_WhenEmailIsMissing_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<AuthenticationQuery>().With(e => e.Email, string.Empty).Create();
            
            // Act
            var result = _authenticationQueryValidator.Validate(query);
            
            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(nameof(AuthenticationQuery.Email), result.Errors.Single().PropertyName);
        }
        
        [Fact]
        public void Validate_WhenEmailIsInvalid_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<AuthenticationQuery>().With(e => e.Email, "not-an-email").Create();
            
            // Act
            var result = _authenticationQueryValidator.Validate(query);
            
            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(nameof(AuthenticationQuery.Email), result.Errors.Single().PropertyName);
        }
        
        [Fact]
        public void Validate_WhenEmailIsValid_ValidationSucceeds()
        {
            // Arrange
            var query = Fixture.Build<AuthenticationQuery>().With(e => e.Email, "agoddamn@email.com").Create();
            
            // Act
            var result = _authenticationQueryValidator.Validate(query);
            
            // Assert
            Assert.True(result.IsValid);
        }
        
        [Fact]
        public void Validate_WhenPasswordIsMissing_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<AuthenticationQuery>()
                .With(e => e.Email, "agoddamn@email.com")
                .With(e => e.Password, string.Empty)
                .Create();
            
            // Act
            var result = _authenticationQueryValidator.Validate(query);
            
            // Assert
            Assert.Single(result.Errors);
            Assert.Equal(nameof(AuthenticationQuery.Password), result.Errors.Single().PropertyName);
        }
        
        [Fact]
        public void Validate_WhenPasswordIsPresent_ValidationSucceeds()
        {
            // Arrange
            var query = Fixture.Build<AuthenticationQuery>().With(e => e.Email, "agoddamn@email.com").Create();
            
            // Act
            var result = _authenticationQueryValidator.Validate(query);
            
            // Assert
            Assert.True(result.IsValid);
        }
    }
}