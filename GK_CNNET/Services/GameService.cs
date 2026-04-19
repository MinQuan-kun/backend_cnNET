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
            var platforms = dto.Platforms ?? new List<string>();
            if (!string.IsNullOrWhiteSpace(dto.Platform))
            {
                platforms = new List<string> { dto.Platform };
            }

            var newGame = new Game
            {
                Name = dto.Name ?? string.Empty,
                Genre = dto.Genre ?? string.Empty,
                Price = dto.Price, // Gán qua thuộc tính ảo tự động ép kiểu chuẩn
                CategoryIds = dto.CategoryIds ?? new List<string>(),
                Publisher = string.IsNullOrWhiteSpace(dto.Publisher) ? (dto.Genre ?? string.Empty) : dto.Publisher,
                Platforms = platforms,
                Languages = dto.Languages ?? new List<string>(),
                Description = dto.Description ?? string.Empty,
                Image = string.IsNullOrWhiteSpace(dto.ImageUrl) ? (dto.Image ?? string.Empty) : dto.ImageUrl,
                Rating = dto.Rating,
                GameUrl = dto.GameUrl ?? string.Empty,
                DownloadLink = dto.DownloadLink ?? string.Empty,
                IsActive = dto.IsActive,
                Slug = dto.Slug ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _games.InsertOneAsync(newGame);
            return MapToReadDto(newGame);
        }

        public async Task<bool> UpdateAsync(string id, GameCreateDto dto)
        {
            var platformValue = dto.Platform;
            if (string.IsNullOrWhiteSpace(platformValue) && dto.Platforms != null && dto.Platforms.Count > 0)
            {
                platformValue = dto.Platforms[0];
            }

            var update = Builders<Game>.Update
                .Set(g => g.Name, dto.Name)
                .Set(g => g.Genre, dto.Genre ?? string.Empty)
                .Set("price", dto.Price) // Đẩy thẳng chuỗi xuống Mongo qua Bson document name
                .Set(g => g.CategoryIds, dto.CategoryIds ?? new List<string>())
                .Set(g => g.Publisher, string.IsNullOrWhiteSpace(dto.Publisher) ? (dto.Genre ?? string.Empty) : dto.Publisher)
                .Set(g => g.Platforms, new List<string> { platformValue ?? string.Empty })
                .Set(g => g.Languages, dto.Languages ?? new List<string>())
                .Set(g => g.Description, dto.Description ?? string.Empty)
                .Set(g => g.Image, string.IsNullOrWhiteSpace(dto.ImageUrl) ? (dto.Image ?? string.Empty) : dto.ImageUrl)
                .Set(g => g.Rating, dto.Rating)
                .Set(g => g.GameUrl, dto.GameUrl ?? string.Empty)
                .Set(g => g.DownloadLink, dto.DownloadLink ?? string.Empty)
                .Set(g => g.IsActive, dto.IsActive)
                .Set(g => g.Slug, dto.Slug ?? string.Empty)
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
                Id = game.Id ?? string.Empty,
                Name = game.Name ?? string.Empty,
                Genre = game.Genre ?? string.Empty,
                Price = game.Price, // Lấy qua thuộc tính ảo (đã xử lý ToString an toàn)
                CategoryIds = game.CategoryIds ?? new List<string>(),
                Publisher = game.Publisher ?? string.Empty,
                Platforms = game.Platforms ?? new List<string>(),
                Platform = game.Platform ?? string.Empty,
                Languages = game.Languages ?? new List<string>(),
                Description = game.Description ?? string.Empty,
                Image = game.Image ?? string.Empty,
                ImageUrl = game.Image ?? string.Empty,
                Rating = game.Rating,
                GameUrl = game.GameUrl ?? string.Empty,
                DownloadLink = game.DownloadLink ?? string.Empty,
                IsActive = game.IsActive,
                Slug = game.Slug ?? string.Empty,
                CreatedAt = game.CreatedAt,
                UpdatedAt = game.UpdatedAt
            };
        }
    }
}