using System.Collections.Generic;
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
    public class UnlikeThreadCommandHandlerTests : UnitTest<UnlikeThreadCommandHandlerTests>, IAsyncLifetime
    {
        private readonly Mock<IContextUserFetcher> _contextUserFetcher = new Mock<IContextUserFetcher>();
        private readonly IMongoCollection<MoolThread> _threads = Mongo.GetCollection<MoolThread>();

        private readonly UnlikeThreadCommandHandler _unlikeThreadCommandHandler;
        
        public UnlikeThreadCommandHandlerTests()
        {
            _unlikeThreadCommandHandler = new UnlikeThreadCommandHandler(_contextUserFetcher.Object, _threads);
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
            var request = Fixture.Build<UnlikeThreadCommand>()
                .With(e => e.Id, ObjectId.GenerateNewId().ToString())
                .Create();

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _unlikeThreadCommandHandler.Handle(request, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_IfThreadIsAlreadyUnlikedByUser_DoesntDoAnything()
        {
            // Arrange
            _contextUserFetcher.SeedUser();
            
            var thread = Fixture.Create<MoolThread>();
            await _threads.InsertOneAsync(thread);

            var command = Fixture.Build<UnlikeThreadCommand>().With(e => e.Id, thread.Id.ToString()).Create();

            // Act
            await _unlikeThreadCommandHandler.Handle(command, CancellationToken.None);
            
            // Assert
            var possiblyModifiedThread = await _threads.Find(e => e.Id == thread.Id).SingleOrDefaultAsync();
            Assert.Equal(thread.LikedBy.Count, possiblyModifiedThread.LikedBy.Count);
        }
        
        [Fact]
        public async Task Handle_IfThreadExistsAndUserHasLikedIt_RemovesUserFromListOfLikers()
        {
            // Arrange
            var user = _contextUserFetcher.SeedUser();

            var thread = Fixture.Build<MoolThread>()
                .With(e => e.LikedBy, new HashSet<ObjectId>(){user.Id})
                .Create();
            await _threads.InsertOneAsync(thread);
            
            var command = Fixture.Build<UnlikeThreadCommand>().With(e => e.Id, thread.Id.ToString()).Create();
            
            // Act
            await _unlikeThreadCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            var modifiedThread = await _threads.Find(e => e.Id == thread.Id).SingleOrDefaultAsync();
            Assert.DoesNotContain(user.Id, modifiedThread.LikedBy);
        }
    }
}