using Grpc.Core;
using GK_CNNET.DTOs;

namespace GK_CNNET.Services
{
    public class GameGrpcService : GameGrpc.GameGrpcBase
    {
        private readonly IGameService _gameService;

        public GameGrpcService(IGameService gameService)
        {
            _gameService = gameService;
        }

        public override async Task<GameList> GetAllGames(GameEmptyRequest request, ServerCallContext context)
        {
            var games = await _gameService.GetAllAsync();
            var response = new GameList();

            foreach (var game in games)
            {
                response.Items.Add(ToGameModel(game));
            }

            return response;
        }

        public override async Task<GameModel> CreateGame(CreateGameRequest request, ServerCallContext context)
        {
            var dto = ToCreateDto(request);
            var created = await _gameService.CreateAsync(dto);
            return ToGameModel(created);
        }

        public override async Task<GameModel> UpdateGame(UpdateGameRequest request, ServerCallContext context)
        {
            var dto = ToCreateDto(request);
            var updated = await _gameService.UpdateAsync(request.Id, dto);
            if (!updated)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Game not found"));
            }

            var game = await _gameService.GetByIdAsync(request.Id);
            if (game == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Game not found"));
            }

            return ToGameModel(game);
        }

        public override async Task<GameDeleteResponse> DeleteGame(GameDeleteRequest request, ServerCallContext context)
        {
            var deleted = await _gameService.DeleteAsync(request.Id);
            return new GameDeleteResponse { Success = deleted };
        }

        private static GameModel ToGameModel(GameReadDto game)
        {
            var parsedPrice = int.TryParse(game.Price, out var priceInt) ? priceInt : 0;

            return new GameModel
            {
                Id = game.Id ?? string.Empty,
                Name = game.Name ?? string.Empty,
                Genre = game.Genre ?? string.Empty,
                Price = parsedPrice,
                ImageUrl = game.ImageUrl ?? game.Image ?? string.Empty,
                Platform = game.Platform ?? string.Empty,
                Description = game.Description ?? string.Empty,
                Rating = game.Rating,
            };
        }

        private static GameCreateDto ToCreateDto(CreateGameRequest request)
        {
            return new GameCreateDto
            {
                Name = request.Name,
                Genre = request.Genre,
                Price = request.Price.ToString(),
                ImageUrl = request.ImageUrl,
                Platform = request.Platform,
                Description = request.Description,
                Rating = request.Rating,
            };
        }

        private static GameCreateDto ToCreateDto(UpdateGameRequest request)
        {
            return new GameCreateDto
            {
                Name = request.Name,
                Genre = request.Genre,
                Price = request.Price.ToString(),
                ImageUrl = request.ImageUrl,
                Platform = request.Platform,
                Description = request.Description,
                Rating = request.Rating,
            };
        }
    }
}
