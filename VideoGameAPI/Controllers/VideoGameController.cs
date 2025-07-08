using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
//using System.Web.Http.Cors;
using VideoGameAPI.Controllers.Data;
using Microsoft.AspNetCore.Cors;
using VideoGameAPI.Models;
namespace VideoGameAPI.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController(VideoGameDbContext context) : ControllerBase
    {
        private readonly VideoGameDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetVideoGames()
        {
            return Ok(await _context.VideoGames
                .Include(g => g.VideoGameDetails)
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<VideoGame>> CreateVideoGame(VideoGame newGame)
        {
            if (newGame is null)
            {
                return BadRequest();
            }

            _context.VideoGames.Add(newGame);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVideoGameById), new { id = newGame.Id }, newGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoGame(int id, VideoGame updatedGame)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
            {
                return NotFound();
            }
            game.Title = updatedGame.Title;
            game.Platform = updatedGame.Platform;
            game.Developer = updatedGame.Developer;
            game.Publisher = updatedGame.Publisher;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGame(int id)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
            {
                return NotFound();
            }
            _context.VideoGames.Remove(game);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
