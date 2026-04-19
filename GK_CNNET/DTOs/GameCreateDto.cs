using System.Collections.Generic;

namespace GK_CNNET.DTOs
{
    public class GameCreateDto
    {
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public string Price { get; set; } = "0";
        public List<string> CategoryIds { get; set; } = new List<string>();
        public string? Publisher { get; set; }
        public List<string> Platforms { get; set; } = new List<string>();
        public string? Platform { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? ImageUrl { get; set; }
        public float Rating { get; set; }
        public string? GameUrl { get; set; }
        public string? DownloadLink { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Slug { get; set; }
    }
}