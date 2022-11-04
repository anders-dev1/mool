using Application.Features;
using AutoFixture;
using FluentValidation.TestHelper;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class ThreadCreationCommandValidatorTests : UnitTest<ThreadCreationCommandValidatorTests>
    {
        private readonly ThreadCreationCommandValidator _threadCreationCommandValidator =
            new ThreadCreationCommandValidator();
        
        [Fact]
        public void Validate_WhenContentIsLessThan10Characters_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<ThreadCreationCommand>().With(e => e.Content, "9   chars").Create();

            // Act
            var result = _threadCreationCommandValidator.TestValidate(query);

            // Assert
            Assert.Single(result.Errors); 
            result.ShouldHaveValidationErrorFor(model => model.Content);
        }

        [Fact]
        public void Validate_WhenContentIsEqualOrMoreThan10_ValidationSucceeds()
        {
            // Arrange
            var query = Fixture.Build<ThreadCreationCommand>().With(e => e.Content, "characters").Create();
            
            // Act
            var result = _threadCreationCommandValidator.TestValidate(query);
            
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}