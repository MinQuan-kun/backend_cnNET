using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GK_CNNET.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("slug")]
        public string Slug { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
    }
}
