using Application.Features;
using AutoFixture;
using FluentValidation.TestHelper;
using MongoDB.Bson;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class LikeThreadCommandValidatorTests : UnitTest<LikeThreadCommandValidatorTests>
    {
        private readonly LikeThreadCommandValidator _likeThreadCommandValidator = new LikeThreadCommandValidator();

        [Fact]
        public void Validate_WhenSpecifiedIdIsNotObjectId_ValidationFails()
        {
            // Arrange
            var command = Fixture.Build<LikeThreadCommand>().With(e => e.Id, string.Empty).Create();

            // Act
            var result = _likeThreadCommandValidator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(model => model.Id);
        }
        
        [Fact]
        public void Validate_WhenSpecifiedIdIsAnObjectId_ValidationSucceeds()
        {
            // Arrange
            var command = Fixture.Build<LikeThreadCommand>().With(e => e.Id, ObjectId.GenerateNewId().ToString()).Create();

            // Act
            var result = _likeThreadCommandValidator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}