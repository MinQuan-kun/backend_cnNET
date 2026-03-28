using System.Collections.Generic;
using System.Threading.Tasks;
using GK_CNNET.DTOs;

namespace GK_CNNET.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameReadDto>> GetAllAsync();
        Task<GameReadDto> GetByIdAsync(string id);
        Task<GameReadDto> CreateAsync(GameCreateDto createDto);
        Task<bool> UpdateAsync(string id, GameCreateDto updateDto);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<GameReadDto>> SearchAsync(string name, string genre);
    }
}