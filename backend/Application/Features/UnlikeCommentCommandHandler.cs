using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Services.Session;
using Application.Settings;
using FluentValidation;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Features
{
    public record UnlikeCommentCommand(string Id) : IRequest<Unit>;
        
    public class UnlikeCommentCommandValidator : AbstractValidator<LikeCommentCommand>
    {
        public UnlikeCommentCommandValidator()
        {
            RuleFor(e => e.Id).MustBeObjectId();
        }
    }
    
    public class UnlikeCommentCommandHandler : IRequestHandler<UnlikeCommentCommand>
    {
        private readonly IContextUserFetcher _contextUserFetcher;
        private readonly IMongoCollection<MoolThread> _threads;

        public UnlikeCommentCommandHandler(
            IContextUserFetcher contextUserFetcher, 
            IMongoCollection<MoolThread> threads)
        {
            _contextUserFetcher = contextUserFetcher;
            _threads = threads;
        }

        public async Task<Unit> Handle(UnlikeCommentCommand request, CancellationToken cancellationToken)
        {
            var user = _contextUserFetcher.FetchOrThrow();

            var commentId = ObjectId.Parse(request.Id);
            var thread = await _threads
                .AsQueryable()
                .SingleOrDefaultAsync(e =>
                        e.Comments.Any(i => i.Id == commentId), 
                    cancellationToken);

            if (thread is null)
            {
                throw new NotFoundException("Could not find thread including comment.");
            }

            var comment = thread.Comments.Single(e => e.Id == commentId);
            if (comment.LikedBy.Contains(user.Id) == false)
            {
                return Unit.Value;
            }
            
            await _threads.FindOneAndUpdateAsync(
                e => e.Id == thread.Id && e.Comments.Any(x => x.Id == commentId),
                Builders<MoolThread>.Update.Pull(e => e.Comments[-1].LikedBy, user.Id), 
                cancellationToken: cancellationToken);
            
            return Unit.Value;
        }
    }
}