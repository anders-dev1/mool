using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Services
{
    public interface IThreadFetcher
    {
        Task<MoolThread> FetchOrThrow(ObjectId id);
    }

    public class ThreadFetcher : IThreadFetcher
    {
        private readonly IMongoCollection<MoolThread> _threads;

        public ThreadFetcher(IMongoCollection<MoolThread> threads)
        {
            _threads = threads;
        }

        public async Task<MoolThread> FetchOrThrow(ObjectId id)
        {
            var thread = await _threads.Find(e => e.Id == id).SingleOrDefaultAsync();
            
            if (thread == null)
            {
                throw new NotFoundException("Could not find requested thread.");
            }
            
            return thread;
        }
    }
}