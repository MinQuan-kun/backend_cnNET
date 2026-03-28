using System.Collections.Generic;
using System.Threading.Tasks;
using GK_CNNET.DTOs;
using GK_CNNET.Services;
using Microsoft.AspNetCore.Mvc;

namespace GK_CNNET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameReadDto>>> GetAll()
        {
            var games = await _gameService.GetAllAsync();
            return Ok(games);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GameReadDto>>> Search([FromQuery] string name, [FromQuery] string genre)
        {
            var games = await _gameService.SearchAsync(name, genre);
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameReadDto>> GetById(string id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound(new { message = "Không tìm thấy Game" });
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GameCreateDto request)
        {
            var createdGame = await _gameService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdGame.Id }, createdGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] GameCreateDto request)
        {
            var isUpdated = await _gameService.UpdateAsync(id, request);
            if (!isUpdated) return NotFound(new { message = "Không tìm thấy Game để cập nhật" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var isDeleted = await _gameService.DeleteAsync(id);
            if (!isDeleted) return NotFound(new { message = "Không tìm thấy Game để xóa" });
            return NoContent();
        }
    }
}