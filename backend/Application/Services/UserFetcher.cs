using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Services
{
    public interface IUserFetcher
    {
        Task<User?> Fetch(ObjectId id);
        Task<IEnumerable<User>> Fetch(IEnumerable<ObjectId> ids);
    }

    public class UserFetcher : IUserFetcher
    {
        private readonly IMongoCollection<User> _users;

        public UserFetcher(IMongoCollection<User> users)
        {
            _users = users;
        }

        public async Task<User?> Fetch(ObjectId id)
        {
            var result = await _users.FindAsync(e => e.Id == id);
            return await result.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> Fetch(IEnumerable<ObjectId> ids)
        {
            var result = await _users.FindAsync(e => ids.Contains(e.Id));
            return await result.ToListAsync();
        }
    }
}