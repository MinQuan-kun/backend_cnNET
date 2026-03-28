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
                Name = dto.Name,
                Price = dto.Price, // Gán qua thuộc tính ảo tự động ép kiểu chuẩn
                CategoryIds = dto.CategoryIds,
                Publisher = dto.Publisher,
                Platforms = dto.Platforms,
                Languages = dto.Languages,
                Description = dto.Description,
                Image = dto.Image,
                GameUrl = dto.GameUrl,
                DownloadLink = dto.DownloadLink,
                IsActive = dto.IsActive,
                Slug = dto.Slug,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _games.InsertOneAsync(newGame);
            return MapToReadDto(newGame);
        }

        public async Task<bool> UpdateAsync(string id, GameCreateDto dto)
        {
            var update = Builders<Game>.Update
                .Set(g => g.Name, dto.Name)
                .Set("price", dto.Price) // Đẩy thẳng chuỗi xuống Mongo qua Bson document name
                .Set(g => g.CategoryIds, dto.CategoryIds)
                .Set(g => g.Publisher, dto.Publisher)
                .Set(g => g.Platforms, dto.Platforms)
                .Set(g => g.Languages, dto.Languages)
                .Set(g => g.Description, dto.Description)
                .Set(g => g.Image, dto.Image)
                .Set(g => g.GameUrl, dto.GameUrl)
                .Set(g => g.DownloadLink, dto.DownloadLink)
                .Set(g => g.IsActive, dto.IsActive)
                .Set(g => g.Slug, dto.Slug)
                .Set(g => g.UpdatedAt, DateTime.UtcNow);

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
            return new GameReadDto
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price, // Lấy qua thuộc tính ảo (đã xử lý ToString an toàn)
                CategoryIds = game.CategoryIds ?? new List<string>(),
                Publisher = game.Publisher,
                Platforms = game.Platforms ?? new List<string>(),
                Languages = game.Languages ?? new List<string>(),
                Description = game.Description,
                Image = game.Image,
                GameUrl = game.GameUrl,
                DownloadLink = game.DownloadLink,
                IsActive = game.IsActive,
                Slug = game.Slug,
                CreatedAt = game.CreatedAt,
                UpdatedAt = game.UpdatedAt
            };
        }
    }
}