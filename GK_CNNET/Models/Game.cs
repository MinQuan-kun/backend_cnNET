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

        //[BsonElement("publisher")]
        //public string Publisher { get; set; }

        [BsonElement("platforms")]
        public List<string> Platforms { get; set; } = new List<string>();

        //[BsonElement("languages")]
        //public List<string> Languages { get; set; } = new List<string>();

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }


        //[BsonIgnore]
        //public string Platform
        //{
        //    get => Platforms != null && Platforms.Count > 0 ? Platforms[0] : string.Empty;
        //    set
        //    {
        //        if (Platforms == null) Platforms = new List<string>();
        //        if (Platforms.Count == 0) Platforms.Add(value);
        //        else Platforms[0] = value;
        //    }
        //}

        //[BsonElement("game_url")]
        //public string GameUrl { get; set; }

        [BsonElement("download_link")]
        public string DownloadLink { get; set; }

        //[BsonElement("is_active")]
        //public bool IsActive { get; set; } = true;

        //[BsonElement("slug")]
        //public string Slug { get; set; }

        //[BsonElement("updated_at")]
        //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //[BsonElement("created_at")]
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}