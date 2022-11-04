using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Services.Session.Model
{
    public record RefreshToken
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        public ObjectId UserId { get; set; }
        public DateTimeOffset Expires { get; set; }

        public RefreshToken(
            ObjectId id,
            ObjectId userId, 
            DateTimeOffset expires)
        {
            Id = id;
            UserId = userId;
            Expires = expires;
        }
    }
}