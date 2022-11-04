using System.Linq;
using Application.Features;
using AutoFixture;
using MongoDB.Bson;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class LikeCommentCommandValidatorTests : UnitTest<LikeCommentCommandValidatorTests>
    {
        private readonly LikeCommentCommandValidator _likeCommentCommandValidator = new LikeCommentCommandValidator();

        [Fact]
        public void Validate_WhenCommentIdIsNotAnObjectId_ValidationFails()
        {
            // Arrange
            var query = Fixture.Build<LikeCommentCommand>()
                .With(e => e.Id, "not an objectId").Create();

            // Act
            var result = _likeCommentCommandValidator.Validate(query);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal(nameof(LikeCommentCommand.Id), result.Errors.Single().PropertyName);
        }
        
        [Fact]
        public void Validate_WhenCommentIdIsAnObjectId_ValidationSucceeds()
        {
            // Arrange
            var query = Fixture.Build<LikeCommentCommand>()
                .With(e => e.Id, ObjectId.GenerateNewId().ToString()).Create();

            // Act
            var result = _likeCommentCommandValidator.Validate(query);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}