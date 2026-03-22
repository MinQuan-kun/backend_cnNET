namespace GK_CNNET.DTOs
{
    public class BookReadDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
