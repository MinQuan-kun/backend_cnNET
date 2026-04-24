using System.Collections.Generic;

namespace GK_CNNET.DTOs
{
    public class GameCreateDto
    {
        public string Name { get; set; } = null!;
        public string Price { get; set; }
        public List<string>? CategoryIds { get; set; }
        public List<string>? Platforms { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? DownloadLink { get; set; }
    }
}