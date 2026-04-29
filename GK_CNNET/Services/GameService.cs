using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GK_CNNET.DTOs;
using GK_CNNET.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GK_CNNET.Services
{
    public class GameService : IGameService
    {
        private readonly IMongoCollection<Game> _games;

        public GameService(IMongoCollection<Game> games)
        {
            _games = games;
        }

        public async Task<IEnumerable<GameReadDto>> GetAllAsync()
        {
            var games = await _games.Find(_ => true).ToListAsync();
            return games.Select(MapToReadDto);
        }

        public async Task<GameReadDto> GetByIdAsync(string id)
        {
            var game = await _games.Find(g => g.Id == id).FirstOrDefaultAsync();
            return game == null ? null : MapToReadDto(game);
        }

        public async Task<GameReadDto> CreateAsync(GameCreateDto dto)
        {
            var newGame = new Game
            {
                Name = dto.Name ?? string.Empty,
                Price = dto.Price ?? "0",

                CategoryIds = dto.CategoryIds ?? new List<string>(),
                Platforms = dto.Platforms ?? new List<string>(),
                Description = dto.Description ?? string.Empty,

                Image = dto.Image ?? string.Empty,
                DownloadLink = dto.DownloadLink ?? string.Empty,
                Slug = Guid.NewGuid().ToString()
                //IsActive = dto.IsActive,
                //CreatedAt = DateTime.UtcNow,
                //UpdatedAt = DateTime.UtcNow
            };

            await _games.InsertOneAsync(newGame);
            return MapToReadDto(newGame);
        }

        public async Task<bool> UpdateAsync(string id, GameCreateDto dto)
        {

            var update = Builders<Game>.Update
                .Set(g => g.Name, dto.Name)
                .Set("price", dto.Price ?? "0")
                .Set(g => g.CategoryIds, dto.CategoryIds ?? new List<string>())
                .Set(g => g.Platforms, dto.Platforms ?? new List<string>())
                .Set(g => g.Description, dto.Description ?? string.Empty)
                .Set(g => g.Image, dto.Image ?? string.Empty)
                .Set(g => g.DownloadLink, dto.DownloadLink ?? string.Empty);
                //.Set(g => g.IsActive, dto.IsActive)
                //.Set(g => g.Slug, dto.Slug ?? string.Empty)
                //.Set(g => g.UpdatedAt, DateTime.UtcNow);

            var result = await _games.UpdateOneAsync(g => g.Id == id, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _games.DeleteOneAsync(g => g.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<GameReadDto>> SearchAsync(string name, string genre)
        {
            var query = _games.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(g => g.Name.ToLower().Contains(name.ToLower()));
            }

            var games = await query.ToListAsync();
            return games.Select(MapToReadDto);
        }

        private static GameReadDto MapToReadDto(Game game)
        {
            if (game == null) return null;

            return new GameReadDto
            {
                Id = game.Id ?? string.Empty,
                Name = game.Name ?? string.Empty,
                Price = game.Price?.ToString() ?? "0",
                CategoryIds = game.CategoryIds ?? new List<string>(),
                Platforms = game.Platforms ?? new List<string>(),

                Platform = (game.Platforms?.Count > 0) ? game.Platforms[0] : string.Empty,

                Description = game.Description ?? string.Empty,
                Image = game.Image ?? string.Empty,


                DownloadLink = game.DownloadLink ?? string.Empty,

                // Slug = game.Slug ?? string.Empty,
                // CreatedAt = game.CreatedAt,
                // UpdatedAt = game.UpdatedAt
            };
        }
    }
}