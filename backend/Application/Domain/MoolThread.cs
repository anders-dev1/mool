using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Domain
{
    public record MoolThread
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTimeOffset Created { get; set; }

        public string Content { get; set; }
        public ObjectId User { get; set; }
        
        public HashSet<ObjectId> LikedBy { get; set; }
        public Comment[] Comments { get; set; }

        public MoolThread(
            string content, 
            ObjectId user)
        {
            Created = DateTimeOffset.Now;
            
            Content = content;
            User = user;

            LikedBy = new HashSet<ObjectId>();
            Comments = Array.Empty<Comment>();
        }
    }
}