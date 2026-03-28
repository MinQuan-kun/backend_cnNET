using System;
using System.Collections.Generic;

namespace GK_CNNET.DTOs
{
    public class GameReadDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public List<string> CategoryIds { get; set; }
        public string Publisher { get; set; }
        public List<string> Platforms { get; set; }
        public List<string> Languages { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string GameUrl { get; set; }
        public string DownloadLink { get; set; }
        public bool IsActive { get; set; }
        public string Slug { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}