using System;
using System.Collections.Generic;

namespace GK_CNNET.DTOs
{
    public class GameReadDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Price { get; set; }
        public List<string>? CategoryIds { get; set; }
        public List<string>? Platforms { get; set; }
        public string? Platform { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? DownloadLink { get; set; }
    }
}