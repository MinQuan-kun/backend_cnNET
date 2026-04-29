using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GK_CNNET.Models
{
    [BsonIgnoreExtraElements]
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public object Price { get; set; }

        [BsonElement("category_ids")]
        public List<string> CategoryIds { get; set; } = new List<string>();


        [BsonElement("platforms")]
        public List<string> Platforms { get; set; } = new List<string>();


        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("download_link")]
        public string DownloadLink { get; set; }

        [BsonElement("slug")]
        public string Slug { get; set; }


    }
}