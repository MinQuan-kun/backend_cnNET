using Grpc.Core;
using GK_CNNET.DTOs;
using MongoDB.Bson;

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
            long.TryParse(game.Price, out var priceLong);

            return new GameModel
            {
                Id = game.Id ?? string.Empty,
                Name = game.Name ?? string.Empty,
                Price = (int)priceLong,
                Description = game.Description ?? string.Empty,
                Image = game.Image ?? string.Empty,
                DownloadLink = game.DownloadLink ?? string.Empty,
            };
        }

        private static GameCreateDto ToCreateDto(CreateGameRequest request)
        {
            return new GameCreateDto
            {
                Name = request.Name,
                Price = request.Price.ToString(),

                CategoryIds = request.CategoryIds.ToList(),
                Platforms = request.Platforms.ToList(),

                Description = request.Description,
                Image = request.Image,
                DownloadLink = request.DownloadLink
            };
        }

        private static GameCreateDto ToCreateDto(UpdateGameRequest request)
        {
            return new GameCreateDto
            {
                Name = request.Name,
                Price = request.Price.ToString(),
                Description = request.Description,
            };
        }
    }
}
