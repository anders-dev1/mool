using System.Collections.Generic;
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
    public class CommentListQueryHandlerTests : UnitTest<CommentListQueryHandlerTests>, IAsyncLifetime
    {
        private readonly Mock<IContextUserFetcher> _contextUserFetcher = new Mock<IContextUserFetcher>();
        private readonly IMongoCollection<MoolThread> _threads = Mongo.GetCollection<MoolThread>();
        private readonly IMongoCollection<User> _users = Mongo.GetCollection<User>();

        private readonly CommentListQueryHandler _commentListQueryHandler;
        
        public CommentListQueryHandlerTests()
        {
            _commentListQueryHandler = new CommentListQueryHandler(
                _contextUserFetcher.Object,
                _threads,
                _users);
        }
        
        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _threads.DeleteManyAsync(FilterDefinition<MoolThread>.Empty);
            await _users.DeleteManyAsync(FilterDefinition<User>.Empty);
        }

        [Theory]
        [InlineData(CommentOrder.OldestFirst)]
        [InlineData(CommentOrder.NewestFirst)]
        public async Task Handle_ReturnsCommentViewModelsForRequestedThreadInGivenOrder(CommentOrder commentOrder)
        {
            // Arrange
            _contextUserFetcher.SeedUser();

            var users = Fixture.CreateMany<User>(10).ToArray();
            await _users.InsertManyAsync(users);
            var usersDict = users.ToDictionary(e => e.Id);
            
            var seededComments = Fixture
                .Build<Comment>()
                .With(e => e.UserId, users.Random().Id)
                .CreateMany(10)
                .OrderBy(e => e.Created)
                .ToArray();

            var thread = Fixture.Build<MoolThread>().With(e => e.Comments, seededComments).Create();
            await _threads.InsertOneAsync(thread);

            var request = new CommentListQuery(thread.Id.ToString(), commentOrder); 
            
            // Act
            var result = await _commentListQueryHandler.Handle(request, CancellationToken.None);
            var returnedComments = result.Comments.ToArray();

            // Assert
            var orderedSeededComments = 
                (commentOrder == CommentOrder.OldestFirst
                ? seededComments.OrderBy(e => e.Created)
                : seededComments.OrderByDescending(e => e.Created))
                .ToArray();

            for (var index = 0; index < orderedSeededComments.Length; index++)
            {
                var seededComment = orderedSeededComments[index];
                var returnedComment = returnedComments[index];
                
                var user = usersDict[seededComment.UserId];
                Assert.Equal(user.FullName(), returnedComment.AuthorName);
                Assert.Equal(seededComment.Content, returnedComment.Content);
                Assert.Equal(seededComment.LikedBy.Count(), returnedComment.Likes);
                Assert.False(returnedComment.LikedByCurrentUser);
            }
        }

        [Fact]
        public async Task Handle_IfCurrentUserHasLikedComment_CommentLikedByCurrentUserIsTrue()
        {
            // Arrange
            var user = _contextUserFetcher.SeedUser();
            var commentingUser = Fixture.Create<User>();
            await _users.InsertManyAsync(new []{user, commentingUser});
            
            var comment = Fixture
                .Build<Comment>()
                .With(e => e.UserId, commentingUser.Id)
                .With(e => e.LikedBy, new HashSet<ObjectId>() { user.Id })
                .Create();
            var thread = Fixture.Build<MoolThread>().With(e => e.Comments, new[] { comment }).Create();
            await _threads.InsertOneAsync(thread);

            var request = new CommentListQuery(thread.Id.ToString());
            
            // Act
            var result = await _commentListQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Comments.Single().LikedByCurrentUser);
        }
        
        [Fact]
        public async Task Handle_ThrowsNotFound_WhenRequestedThreadDoesNotExist()
        {
            // Arrange
            _contextUserFetcher.SeedUser();
            var request = new CommentListQuery(ObjectId.GenerateNewId().ToString()); 
            
            // Act + assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _commentListQueryHandler.Handle(request, CancellationToken.None));
        }
    }
}