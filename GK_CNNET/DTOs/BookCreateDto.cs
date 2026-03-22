namespace GK_CNNET.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string Author { get; set; } = null!;
        public int Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
