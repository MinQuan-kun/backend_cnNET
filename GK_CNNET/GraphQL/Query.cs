using GK_CNNET.Models;
using GK_CNNET.DTOs;
using GK_CNNET.Services;
using MongoDB.Driver;
using HotChocolate;

namespace GK_CNNET.GraphQL
{
    public class Query
    {
        public async Task<List<Book>> GetBooks([Service] IMongoCollection<Book> collection)
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<Book?> GetBookById(string id, [Service] IMongoCollection<Book> collection)
        {
            return await collection.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetBooksWithImages([Service] IMongoCollection<Book> collection)
        {
            return await collection.Find(b => b.ImageUrl != null).ToListAsync();
        }

        public async Task<IEnumerable<GameReadDto>> Games([Service] IGameService gameService)
        {
            return await gameService.GetAllAsync();
        }

        public async Task<GameReadDto?> GameById(string id, [Service] IGameService gameService)
        {
            return await gameService.GetByIdAsync(id);
        }

    }
 }