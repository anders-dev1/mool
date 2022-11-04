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
    public record UnlikeThreadCommand(string Id) : IRequest<Unit>;
    
    public class UnlikeThreadCommandValidator : AbstractValidator<UnlikeThreadCommand>
    {
        public UnlikeThreadCommandValidator()
        {
            RuleFor(e => e.Id).MustBeObjectId();
        }
    }
    
    public class UnlikeThreadCommandHandler : IRequestHandler<UnlikeThreadCommand>
    {
        private readonly IContextUserFetcher _contextUserFetcher;
        private readonly IMongoCollection<MoolThread> _threads;

        public UnlikeThreadCommandHandler(
            IContextUserFetcher contextUserFetcher, 
            IMongoCollection<MoolThread> threads)
        {
            _contextUserFetcher = contextUserFetcher;
            _threads = threads;
        }
        
        public async Task<Unit> Handle(UnlikeThreadCommand request, CancellationToken cancellationToken)
        {
            var threadId = ObjectId.Parse(request.Id);
            var user = _contextUserFetcher.FetchOrThrow();
            var thread = await _threads
                .AsQueryable()
                .SingleOrDefaultAsync(e => e.Id == threadId, cancellationToken);

            if (thread is null)
            {
                throw new NotFoundException("Could not find requested thread to like.");
            }

            if (thread.LikedBy.Contains(user.Id) == false)
            {
                return Unit.Value;
            }

            await _threads.UpdateOneAsync(
                e => e.Id == threadId, Builders<MoolThread>.Update.Pull(e => e.LikedBy, user.Id), 
                cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}