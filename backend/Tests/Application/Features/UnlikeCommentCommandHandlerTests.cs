using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Features;
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
    public class UnlikeCommentCommandHandlerTests : UnitTest<UnlikeCommentCommandHandlerTests>, IAsyncLifetime
    {
        private readonly Mock<IContextUserFetcher> _contextUserFetcher = new Mock<IContextUserFetcher>();
        private readonly IMongoCollection<MoolThread> _threads = Mongo.GetCollection<MoolThread>();

        private readonly UnlikeCommentCommandHandler _unlikeCommentCommandHandler;
        
        public UnlikeCommentCommandHandlerTests()
        {
            _unlikeCommentCommandHandler = new UnlikeCommentCommandHandler(_contextUserFetcher.Object, _threads);
        }
        
        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _threads.DeleteManyAsync(FilterDefinition<MoolThread>.Empty);
        }

        [Fact]
        public async Task Handle_IfSpecifiedThreadIdDoesNotReferToAStoredThread_ThrowsNotFound()
        {
            // Arrange
            var command = Fixture.Build<UnlikeCommentCommand>()
                .With(e => e.Id, ObjectId.GenerateNewId().ToString())
                .Create();

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _unlikeCommentCommandHandler.Handle(command, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_IfSpecifiedCommentIdDoesNotReferToAStoredComment_ThrowsNotFound()
        {
            // Arrange
            var thread = Fixture.Create<MoolThread>();
            await _threads.InsertOneAsync(thread);
            
            var command = new UnlikeCommentCommand(ObjectId.GenerateNewId().ToString());

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _unlikeCommentCommandHandler.Handle(command, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_IfCommentIsNotLikedByUser_DoesntDoAnything()
        {
            // Arrange
            var user = _contextUserFetcher.SeedUser();

            var comment = Fixture.Create<Comment>();
            var thread = Fixture
                .Build<MoolThread>()
                .With(e => e.Comments, new[] { comment })
                .Create();
            await _threads.InsertOneAsync(thread);

            var command = new UnlikeCommentCommand(comment.Id.ToString());
            
            // Act
            await _unlikeCommentCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            var possiblyModifiedThread = await _threads.Find(e => e.Id == thread.Id).SingleOrDefaultAsync();
            Assert.Equal(thread.Comments.Length, possiblyModifiedThread.Comments.Length);
        }
        
        [Fact]
        public async Task Handle_IfThreadAndCommentExistAndUserHasLikedIt_RemovesUserFromListOfLikers()
        {
            // Arrange
            var user = _contextUserFetcher.SeedUser();

            var comment = Fixture
                .Build<Comment>()
                .With(e => e.LikedBy, new HashSet<ObjectId>(){ObjectId.GenerateNewId(), ObjectId.GenerateNewId()})
                .Create();
            var thread = Fixture
                .Build<MoolThread>()
                .With(e => e.Comments, new[] { comment })
                .Create();
            await _threads.InsertOneAsync(thread);

            var command = new UnlikeCommentCommand(comment.Id.ToString());
            
            // Act
            await _unlikeCommentCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            var possiblyModifiedThread = await _threads.Find(e => e.Id == thread.Id).SingleOrDefaultAsync();
            Assert.DoesNotContain(user.Id, possiblyModifiedThread.Comments.Single().LikedBy);
        }
    }
}