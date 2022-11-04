using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Services.Session;
using Application.Settings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Features
{
    public record LikeThreadCommand(string Id) : IRequest<Unit>;

    public class LikeThreadCommandValidator : AbstractValidator<LikeThreadCommand>
    {
        public LikeThreadCommandValidator()
        {
            RuleFor(e => e.Id).MustBeObjectId();
        }
    }

    public class LikeThreadCommandHandler : IRequestHandler<LikeThreadCommand>
    {
        private readonly IContextUserFetcher _contextUserFetcher;
        private readonly IMongoCollection<MoolThread> _threads;

        public LikeThreadCommandHandler(
            IContextUserFetcher contextUserFetcher, 
            IMongoCollection<MoolThread> threads)
        {
            _contextUserFetcher = contextUserFetcher;
            _threads = threads;
        }

        public async Task<Unit> Handle(LikeThreadCommand request, CancellationToken cancellationToken)
        {
            var user = _contextUserFetcher.FetchOrThrow();
            
            var threadId = ObjectId.Parse(request.Id);
            var thread = await _threads
                .AsQueryable()
                .SingleOrDefaultAsync(e => e.Id == threadId, cancellationToken);

            if (thread is null)
            {
                throw new NotFoundException("Could not find requested thread to like.");
            }

            if (thread.LikedBy.Contains(user.Id))
            {
                return Unit.Value;
            }

            await _threads.UpdateOneAsync(
                e => e.Id == threadId, Builders<MoolThread>.Update.Push(e => e.LikedBy, user.Id), 
                cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}