using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Services;
using Application.Services.Session;
using Application.Settings;
using FluentValidation;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Features
{
    public record CommentListQuery(
        string ThreadId, 
        CommentOrder CommentOrder = CommentOrder.OldestFirst) : IRequest<CommentList>;

    public enum CommentOrder
    {
        OldestFirst,
        NewestFirst
    }
    
    public record CommentList(IEnumerable<CommentViewModel> Comments);
    public record CommentViewModel(
        string Id, 
        string AuthorName,
        string Content,
        int Likes, 
        bool LikedByCurrentUser);
    
    public class CommentListQueryValidator : AbstractValidator<CommentListQuery>
    {
        public CommentListQueryValidator()
        {
            RuleFor(e => e.ThreadId).MustBeObjectId();
        }
    }
    
    public class CommentListQueryHandler : IRequestHandler<CommentListQuery, CommentList>
    {
        private readonly IContextUserFetcher _contextUserFetcher;
        private readonly IMongoCollection<MoolThread> _threads;
        private readonly IMongoCollection<User> _users;

        public CommentListQueryHandler(
            IContextUserFetcher contextUserFetcher,
            IMongoCollection<MoolThread> threads,
            IMongoCollection<User> users)
        {
            _contextUserFetcher = contextUserFetcher;
            _threads = threads;
            _users = users;
        }
        
        public async Task<CommentList> Handle(CommentListQuery request, CancellationToken cancellationToken)
        {
            var requestingUser = _contextUserFetcher.FetchOrThrow();
            var threadId = ObjectId.Parse(request.ThreadId);
            var thread = await _threads
                .AsQueryable()
                .SingleOrDefaultAsync(e => e.Id == threadId, cancellationToken);
            
            if (thread is null)
            {
                throw new NotFoundException("Could not find requested thread.");
            }
            
            // Get users from comments and save them in dict for later lookup.
            var userIds = thread.Comments.Select(e => e.UserId).ToArray();
            var users = await _users
                .AsQueryable()
                .Where(e => userIds.Contains(e.Id))
                .ToListAsync(cancellationToken);
            
            var userDict = users.ToDictionary(e => e.Id);

            var commentModels = thread.Comments.ToList();
            if (request.CommentOrder == CommentOrder.NewestFirst)
            {
                commentModels = commentModels.OrderByDescending(e => e.Created).ToList();
            }
            
            // Build the viewmodels using previously fetched entities. 
            var commentViewModels = commentModels.Select(e =>
            {
                var user = userDict[e.UserId];
                return new CommentViewModel(
                    e.Id.ToString(),
                    user.FullName(),
                    e.Content,
                    e.LikedBy.Count, 
                    e.LikedBy.Contains(requestingUser.Id));
            });

            return new CommentList(commentViewModels);
        }
    }
}