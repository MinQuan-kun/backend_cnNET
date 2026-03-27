using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GK_CNNET.Models
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("category_ids")]
        public List<string> CategoryIds { get; set; } = new List<string>();

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }
        [BsonElement("game_url")]
        public string GameUrl { get; set; }
    }
}
