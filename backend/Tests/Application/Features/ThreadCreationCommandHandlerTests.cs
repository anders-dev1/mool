using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Features;
using Application.Services.Session;
using AutoFixture;
using MongoDB.Driver;
using Moq;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class ThreadCreationCommandHandlerTests : UnitTest<ThreadCreationCommandHandlerTests>
    {
        private readonly Mock<IContextUserFetcher> _contextUserFetcher = new Mock<IContextUserFetcher>();
        private readonly IMongoCollection<MoolThread> _threadsCollection = Mongo.GetCollection<MoolThread>();

        private readonly ThreadCreationCommandHandler _threadCreationCommandHandler;

        public ThreadCreationCommandHandlerTests()
        {
            _threadCreationCommandHandler = new ThreadCreationCommandHandler(_contextUserFetcher.Object, _threadsCollection);
        }

        [Fact]
        public async Task Handle_InsertsThreadWithContentAndUserId()
        {
            // Arrange
            var user = Fixture.Create<User>();
            _contextUserFetcher.Setup(e => e.FetchOrThrow()).Returns(user);
            
            var query = Fixture
                .Build<ThreadCreationCommand>()
                .With(e => e.Content, "This is my thread content")
                .Create();

            // Act
            await _threadCreationCommandHandler.Handle(query, CancellationToken.None);

            // Assert
            var threads = await _threadsCollection.Find(FilterDefinition<MoolThread>.Empty).ToListAsync();
            Assert.Single(threads);

            var thread = threads.Single();
            Assert.Equal(user.Id, thread.User);
            Assert.Equal(query.Content, thread.Content);
        }
    }
}