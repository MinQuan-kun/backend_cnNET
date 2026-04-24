using GK_CNNET.Models;
using GK_CNNET.DTOs;
using GK_CNNET.Services;
using MongoDB.Driver;
using HotChocolate;

namespace GK_CNNET.GraphQL
{
    public class Query
    {
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