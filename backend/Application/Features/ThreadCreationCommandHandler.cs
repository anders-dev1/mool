using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Services.Session;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace Application.Features
{
    public record ThreadCreationCommand(string Content) : IRequest<Unit>;

    public class ThreadCreationCommandValidator : AbstractValidator<ThreadCreationCommand>
    {
        public ThreadCreationCommandValidator()
        {
            RuleFor(e => e.Content).MinimumLength(10);
        }
    }
    
    public class ThreadCreationCommandHandler : IRequestHandler<ThreadCreationCommand>
    {
        private readonly IContextUserFetcher _contextUserFetcher;
        private readonly IMongoCollection<MoolThread> _threads;

        public ThreadCreationCommandHandler(
            IContextUserFetcher contextUserFetcher,
            IMongoCollection<MoolThread> threads)
        {
            _contextUserFetcher = contextUserFetcher;
            _threads = threads;
        }
        
        public async Task<Unit> Handle(ThreadCreationCommand request, CancellationToken cancellationToken)
        {
            var user = _contextUserFetcher.FetchOrThrow();
            var thread = new MoolThread(request.Content, user.Id);

            await _threads.InsertOneAsync(thread, cancellationToken: cancellationToken);
            
            return Unit.Value;
        }
    }
}