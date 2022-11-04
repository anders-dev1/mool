using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Services.Session;
using MediatR;
using MongoDB.Driver;

namespace Application.Features
{
    public record ThreadListQuery() : IRequest<ThreadList>;

    public record ThreadList(IEnumerable<MoolThreadViewModel> Threads);
    public record MoolThreadViewModel(
        string Id, 
        DateTimeOffset Created, 
        string? Author, 
        string Content,
        int Likes,
        bool LikedByCurrentUser, 
        int Comments);

    public class ThreadListQueryHandler : IRequestHandler<ThreadListQuery, ThreadList>
    {
        private readonly IContextUserFetcher _contextUserFetcher;
        
        private readonly IMongoCollection<MoolThread> _threads;
        private readonly IMongoCollection<User> _users;

        public ThreadListQueryHandler(
            IContextUserFetcher contextUserFetcher,
            IMongoCollection<MoolThread> threads,
            IMongoCollection<User> users)
        {
            _contextUserFetcher = contextUserFetcher;
            _threads = threads;
            _users = users;
        }
        
        public async Task<ThreadList> Handle(ThreadListQuery request, CancellationToken cancellationToken)
        {
            var threads = await _threads
                .Find(FilterDefinition<MoolThread>.Empty)
                .SortByDescending(e => e.Created)
                .Limit(10)
                .ToListAsync(cancellationToken);

            var userIds = threads.Select(e => e.User);
            var userFilter = Builders<User>.Filter.In(e => e.Id, userIds);

            var users = await _users.Find(userFilter).ToListAsync(cancellationToken: cancellationToken);

            var requestUserId = _contextUserFetcher.FetchOrThrow().Id;
            
            var viewModels = threads.Select(e =>
            {
                var user = users.FirstOrDefault(x => e.User == x.Id);
                return new MoolThreadViewModel(
                    e.Id.ToString(),
                    e.Created,
                    user?.FullName(),
                    e.Content,
                    e.LikedBy.Count,
                    e.LikedBy.Contains(requestUserId),
                    e.Comments.Length
                );
            });

            return new ThreadList(viewModels);
        }
    }
}