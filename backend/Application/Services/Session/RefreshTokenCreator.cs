using System;
using System.Threading.Tasks;
using Application.Services.Session.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Services.Session
{
    public interface IRefreshTokenCreator
    {
        Task<string> Create(string userId);
    }

    public class RefreshTokenCreator : IRefreshTokenCreator
    {
        private readonly IMongoCollection<RefreshToken> _tokens;

        public RefreshTokenCreator(IMongoCollection<RefreshToken> tokens)
        {
            _tokens = tokens;
        }
        
        public async Task<string> Create(string userId)
        {
            var tokenId = ObjectId.GenerateNewId();
            var token = new RefreshToken(
                tokenId,
                ObjectId.Parse(userId),
                DateTimeOffset.Now.AddYears(1));

            await _tokens.InsertOneAsync(token);

            return tokenId.ToString();
        }
    }
}