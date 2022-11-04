using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
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
    public class ThreadListQueryHandlerTests : UnitTest<ThreadListQueryHandlerTests>, IAsyncLifetime
    {
        private readonly Mock<IContextUserFetcher> _contextUserFetcher = new Mock<IContextUserFetcher>();
        
        private readonly IMongoCollection<MoolThread> _threads = Mongo.GetCollection<MoolThread>();
        private readonly IMongoCollection<User> _users = Mongo.GetCollection<User>();

        private readonly ThreadListQueryHandler _threadListQueryHandler;
        
        public ThreadListQueryHandlerTests()
        {
            _threadListQueryHandler = new ThreadListQueryHandler(_contextUserFetcher.Object, _threads, _users);
        }
        
        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _threads.DeleteManyAsync(FilterDefinition<MoolThread>.Empty);
            await _users.DeleteManyAsync(FilterDefinition<User>.Empty);
        }

        [Fact]
        public async Task Handle_ListsTheTenMostRecentlyCreatedThreads()
        {
            // Arrange
            _contextUserFetcher.SeedUser();
            
            // Insert users.
            var users = Fixture.CreateMany<User>(5).ToList();
            await _users.InsertManyAsync(users);

            // Insert threads.
            var random = new Random();
            var threads = Fixture
                .Build<MoolThread>()
                .With(e => e.User, () => users[random.Next(users.Count)].Id)
                .With(e => e.Comments, Fixture.CreateMany<Comment>().ToArray())
                .CreateMany(50)
                .ToList();
            await _threads.InsertManyAsync(threads);

            var recentlyCreatedTenThreads = threads
                .OrderByDescending(e => e.Created)
                .Take(10)
                .ToList();
            
            // Act
            var result = await _threadListQueryHandler.Handle(new ThreadListQuery(), CancellationToken.None);
            var resultThreads = result.Threads.ToList();

            // Assert
            Assert.Equal(10, result.Threads.Count());
            for (var index = 0; index < result.Threads.Count(); index++)
            {
                var recentlyCreatedThread = recentlyCreatedTenThreads[index];
                var resultThread = resultThreads[index];
                
                Assert.Equal(recentlyCreatedThread.Id.ToString(), resultThread.Id);
                Assert.Equal(recentlyCreatedThread.Content, resultThread.Content);
                Assert.Equal(recentlyCreatedThread.Created, resultThread.Created);
                Assert.Equal(recentlyCreatedThread.LikedBy.Count, resultThread.Likes);
                Assert.Equal(recentlyCreatedThread.Comments.Length, resultThread.Comments);

                // We need to get the user using the original user reference from the thread.
                // The user reference isn't in the viewmodel, only the internal thread.
                var user = users.Single(e => e.Id == recentlyCreatedThread.User);
                Assert.Equal(user.FullName(), resultThread.Author);
            }
        }

        [Fact]
        public async Task Handle_IfCurrentUserLikesAThread_LikedByCurrentUserIsTrue()
        {
            // Arrange
            var requestingUser = _contextUserFetcher.SeedUser();

            await _users.InsertOneAsync(requestingUser);

            var thread = Fixture.Create<MoolThread>();
            thread.LikedBy.Add(requestingUser.Id);
            await _threads.InsertOneAsync(thread);

            // Act
            var result = await _threadListQueryHandler.Handle(new ThreadListQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.Threads.Single().LikedByCurrentUser);
        }
        
        [Fact]
        public async Task Handle_IfCurrentUserDoesntLikeAThread_LikedByCurrentUserIsFalse()
        {
            // Arrange
            var requestingUser = _contextUserFetcher.SeedUser();

            await _users.InsertOneAsync(requestingUser);

            var thread = Fixture.Create<MoolThread>();
            await _threads.InsertOneAsync(thread);

            // Act
            var result = await _threadListQueryHandler.Handle(new ThreadListQuery(), CancellationToken.None);

            // Assert
            Assert.False(result.Threads.Single().LikedByCurrentUser);
        }
    }
}