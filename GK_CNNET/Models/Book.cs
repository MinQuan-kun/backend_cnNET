using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GK_CNNET.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Price { get; set; }
    public string? ImageUrl { get; set; }

    public Book(string title, string author, int price, string? imageUrl = null)
    {
        Title = title;
        Author = author;
        Price = price;
        ImageUrl = imageUrl;
    }

    public Book() { }
}