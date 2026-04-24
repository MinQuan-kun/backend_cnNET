using GK_CNNET.Models;
using GK_CNNET.DTOs;
using GK_CNNET.Services;
using MongoDB.Driver;

namespace GK_CNNET.GraphQL
{
    public class Mutation
    {

        public async Task<GameReadDto> CreateGameAsync(
            GameCreateDto input,
            [Service] IGameService gameService)
        {
            return await gameService.CreateAsync(input);
        }

        public async Task<GameReadDto?> UpdateGameAsync(
            string id,
            GameCreateDto input,
            [Service] IGameService gameService)
        {
            var updated = await gameService.UpdateAsync(id, input);
            if (!updated)
            {
                return null;
            }

            return await gameService.GetByIdAsync(id);
        }

        public async Task<bool> DeleteGameAsync(
            string id,
            [Service] IGameService gameService)
        {
            return await gameService.DeleteAsync(id);
        }
    }
}