using System.Linq;
using Application.Features;
using MongoDB.Bson;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class CommentCreationCommandValidatorTests : UnitTest<CommentCreationCommandValidatorTests>
    {
        private readonly CommentCreationCommandValidator _commentCreationCommandValidator =
            new CommentCreationCommandValidator();

        private const string ValidContent = "characters";
        
        [Fact]
        public void Validate_WhenThreadIdIsNotAnObjectId_ValidationFails()
        {
            // Arrange
            var query = new CommentCreationCommand("Not an object Id", ValidContent);
            
            // Act
            var result = _commentCreationCommandValidator.Validate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(nameof(CommentCreationCommand.ThreadId), result.Errors.Single().PropertyName);
        }
        
        [Fact]
        public void Validate_WhenContentIsBelow10InLength_ValidationFails()
        {
            // Arrange
            var query = new CommentCreationCommand(ObjectId.GenerateNewId().ToString(), "9   chars");
            
            // Act
            var result = _commentCreationCommandValidator.Validate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(nameof(CommentCreationCommand.Content), result.Errors.Single().PropertyName);
        }
        
        [Fact]
        public void Validate_WhenThreadIdIsObjectIdAndContentIsEqualOrAbove10InLength_ValidationSucceeds()
        {
            // Arrange
            var query = new CommentCreationCommand(ObjectId.GenerateNewId().ToString(), ValidContent);
            
            // Act
            var result = _commentCreationCommandValidator.Validate(query);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}