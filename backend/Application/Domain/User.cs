using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Domain
{
    public record User
    {
        [BsonId]
        public ObjectId Id { get; set; }
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User(
            string firstname,
            string lastname,
            string email, 
            string password)
        {
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Password = password;
        }

        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}