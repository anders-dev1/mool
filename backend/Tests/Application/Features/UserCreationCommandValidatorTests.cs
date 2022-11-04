using Application.Features;
using AutoFixture;
using FluentValidation.TestHelper;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class UserCreationCommandValidatorTests : UnitTest<UserCreationCommandValidatorTests>
    {
        private readonly UserCreationCommandValidator _userCreationCommandValidator = new UserCreationCommandValidator();

        [Fact]
        public void Validate_WhenEmailIsMissing_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<UserCreationCommand>().With(e => e.Email, string.Empty).Create();
            
            // Act
            var result = _userCreationCommandValidator.TestValidate(query);
            
            // Assert
            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor(model => model.Email);
        }
        
        [Fact]
        public void Validate_WhenEmailIsInvalid_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<UserCreationCommand>().With(e => e.Email, "not-an-email").Create();
            
            // Act
            var result = _userCreationCommandValidator.TestValidate(query);
            
            // Assert
            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor(model => model.Email);
        }

        [Fact]
        public void Validate_WhenPasswordIsMissing_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<UserCreationCommand>()
                .With(e => e.Email, "agoddamn@email.com")
                .With(e => e.Password, string.Empty)
                .Create();
            
            // Act
            var result = _userCreationCommandValidator.TestValidate(query);
            
            // Assert
            result.ShouldHaveValidationErrorFor(model => model.Password);
        }

        [Fact]
        public void Validate_WhenFirstNameIsMissing_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<UserCreationCommand>()
                .With(e => e.Email, "agoddamn@email.com")
                .With(e => e.FirstName, string.Empty)
                .Create();
            
            // Act
            var result = _userCreationCommandValidator.TestValidate(query);
            
            // Assert
            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor(model => model.FirstName);
        }
        
        [Fact]
        public void Validate_WhenLastNameIsMissing_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<UserCreationCommand>()
                .With(e => e.Email, "agoddamn@email.com")
                .With(e => e.LastName, string.Empty)
                .Create();
            
            // Act
            var result = _userCreationCommandValidator.TestValidate(query);
            
            // Assert
            Assert.Single(result.Errors);
            result.ShouldHaveValidationErrorFor(model => model.LastName);
        }

        [Fact]
        public void Validate_EmailIsValidAndAllOtherFieldsArePresent_ValidationSucceeds()
        {
            // Arrange
            var query = Fixture.Build<UserCreationCommand>().With(e => e.Email, "agoddamn@email.com").Create();
            
            // Act
            var result = _userCreationCommandValidator.TestValidate(query);
            
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}