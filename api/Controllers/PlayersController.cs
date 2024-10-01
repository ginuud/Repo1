using api.Data.Repos;
using api.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController(PlayersRepo repo) : ControllerBase()
    {
        private readonly PlayersRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var result = await repo.GetAllPlayers();
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SavePlayer([FromBody] Player player){
            var playerExists = await repo.PlayerExistsInDb(player.Id);
            if (playerExists){
                return Conflict();
            }
            var result = repo.SavePlayerToDb(player);
            return CreatedAtAction(nameof(SavePlayer), new {player.Id}, result);
        }
    }
}