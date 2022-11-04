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

namespace Application.Features
{
    public record CommentCreationCommand(
        string ThreadId, 
        string Content) : IRequest<Unit>;
        
    public class CommentCreationCommandValidator : AbstractValidator<CommentCreationCommand>
    {
        public CommentCreationCommandValidator()
        {
            RuleFor(e => e.ThreadId).MustBeObjectId();
            RuleFor(e => e.Content).MinimumLength(10);
        }
    }
    
    public class CommentCreationCommandHandler : IRequestHandler<CommentCreationCommand>
    {
        private readonly IContextUserFetcher _contextUserFetcher;
        private readonly IMongoCollection<MoolThread> _threads;
        private readonly IUtcNowGetter _utcNowGetter;

        public CommentCreationCommandHandler(
            IContextUserFetcher contextUserFetcher,
            IMongoCollection<MoolThread> threads,
            IUtcNowGetter utcNowGetter)
        {
            _contextUserFetcher = contextUserFetcher;
            _threads = threads;
            _utcNowGetter = utcNowGetter;
        }

        public async Task<Unit> Handle(CommentCreationCommand command, CancellationToken cancellationToken)
        {
            var requestingUser = _contextUserFetcher.FetchOrThrow();
            var comment = new Comment(command.Content, requestingUser.Id, _utcNowGetter.UtcNow());
            
            var threadId = ObjectId.Parse(command.ThreadId);
            
            var result = await _threads.UpdateOneAsync(
                e => e.Id == threadId, 
                Builders<MoolThread>.Update.AddToSet(e => e.Comments, comment),
                cancellationToken: cancellationToken);

            if (result.ModifiedCount == 0)
            {
                throw new NotFoundException("Could not find requested thread.");
            }

            return Unit.Value;
        }
    }
}