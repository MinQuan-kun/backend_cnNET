using GK_CNNET.Models;
using GK_CNNET.DTOs;
using MongoDB.Driver;

namespace GK_CNNET.GraphQL
{
    public class Mutation
    {
        public async Task<BookReadDto> CreateBookAsync(
        BookCreateDto input,
        [Service] IMongoCollection<Book> collection)
        {
            var newBook = new Book
            {
                Title = input.Title,
                Author = input.Author,
                Price = input.Price,
                ImageUrl = input.ImageUrl
            };

            await collection.InsertOneAsync(newBook);

            return new BookReadDto
            {
                Id = newBook.Id!,
                Title = newBook.Title,
                Author = newBook.Author,
                Price = newBook.Price,
                ImageUrl = newBook.ImageUrl
            };
        }
        public async Task<bool> DeleteBookAsync(string id, [Service] IMongoCollection<Book> collection)
        {
            var result = await collection.DeleteOneAsync(b => b.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<BookReadDto> UpdateBookAsync(
        string id,
        BookCreateDto input,
        [Service] IMongoCollection<Book> collection)
        {
            var updatedBook = new Book
            {
                Id = id,
                Title = input.Title,
                Author = input.Author,
                Price = input.Price,
                ImageUrl = input.ImageUrl
            };

            await collection.ReplaceOneAsync(b => b.Id == id, updatedBook);

            return new BookReadDto
            {
                Id = id,
                Title = updatedBook.Title,
                Author = updatedBook.Author,
                Price = updatedBook.Price,
                ImageUrl = updatedBook.ImageUrl
            };
        }
    }
}