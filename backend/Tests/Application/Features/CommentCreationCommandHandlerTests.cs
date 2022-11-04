using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Features;
using Application.Services;
using Application.Services.Session;
using AutoFixture;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class CommentCreationCommandHandlerTests : UnitTest<CommentCreationCommandHandlerTests>, IAsyncLifetime
    {
        private readonly Mock<IContextUserFetcher> _contextUserFetcher = new Mock<IContextUserFetcher>();
        private readonly IMongoCollection<MoolThread> _threads = Mongo.GetCollection<MoolThread>();
        private readonly Mock<IUtcNowGetter> _utcNowGetter = new Mock<IUtcNowGetter>();
        
        private readonly CommentCreationCommandHandler _commentCreationCommandHandler;
        
        public CommentCreationCommandHandlerTests()
        {
            _commentCreationCommandHandler = new CommentCreationCommandHandler(
                _contextUserFetcher.Object, 
                _threads,
                _utcNowGetter.Object);
        }
        
        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _threads.DeleteManyAsync(FilterDefinition<MoolThread>.Empty);
        }

        [Fact]
        public async Task Handle_AddsCommentToSpecifiedThread()
        {
            // Arrange
            var commentingUser = _contextUserFetcher.SeedUser();
            var seededNow = _utcNowGetter.SeedNow();
            
            var thread = Fixture.Create<MoolThread>();
            await _threads.InsertOneAsync(thread);
            
            var request = new CommentCreationCommand(thread.Id.ToString(), "This is a specific comment.");

            // Act
            await _commentCreationCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            var modifiedThread = _threads.Find(e => e.Id == thread.Id).Single();
            var createdComment = modifiedThread.Comments.Last();
            
            Assert.Equal(request.Content, createdComment.Content);
            Assert.Equal(commentingUser.Id, createdComment.UserId);
            Assert.Empty(createdComment.LikedBy);
            Assert.Equal(seededNow, createdComment.Created);
        }

        [Fact]
        public async Task Handle_IfSpecifiedThreadDoesntExist_ThrowsNotFoundException()
        {
            // Arrange
            _contextUserFetcher.SeedUser();
            var request = new CommentCreationCommand(ObjectId.GenerateNewId().ToString(), "This won't get posted.");

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _commentCreationCommandHandler.Handle(request, CancellationToken.None));
        }
    }
}