using System.Linq;
using Application.Features;
using MongoDB.Bson;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class UnlikeThreadCommandValidatorTests : UnitTest<UnlikeThreadCommandValidatorTests>
    {
        private readonly UnlikeThreadCommandValidator
            _unlikeThreadCommandValidator = new UnlikeThreadCommandValidator();

        [Fact]
        public void Validate_WhenIdIsNotAnObjectId_ValidationFails()
        {
            // Arrange
            var query = new UnlikeThreadCommand("Not an objectId");

            // Act
            var result = _unlikeThreadCommandValidator.Validate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(nameof(UnlikeThreadCommand.Id), result.Errors.Single().PropertyName);
        }
        
        [Fact]
        public void Validate_WhenIdIsAnObjectId_ValidationSucceeds()
        {
            // Arrange
            var query = new UnlikeThreadCommand(ObjectId.GenerateNewId().ToString());

            // Act
            var result = _unlikeThreadCommandValidator.Validate(query);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}