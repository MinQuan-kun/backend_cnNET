using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GK_CNNET.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("role")]
        public string Role { get; set; } = "user";

        [BsonElement("status")]
        public string Status { get; set; } = "active";
    }
}
