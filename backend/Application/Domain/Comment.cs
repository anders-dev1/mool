using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Domain
{
    public record Comment
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Content { get; set; }
        public ObjectId UserId { get; set; }
        public HashSet<ObjectId> LikedBy { get; set; }

        public Comment(
            string content,
            ObjectId userId,
            DateTimeOffset created)
        {
            Id = ObjectId.GenerateNewId();
            Created = created;
            Content = content;
            UserId = userId;
            LikedBy = new HashSet<ObjectId>();
        }
    }
}