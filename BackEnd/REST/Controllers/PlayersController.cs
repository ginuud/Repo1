using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using REST.Data.Repos;
using REST.Models.Classes;

namespace REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController(PlayersRepo repo) : ControllerBase
    {
        private readonly PlayersRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repo.GetAllPlayers();
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id){
            var player = await repo.GetPlayerById(id);
            if (player == null){
                return NotFound();
            }
            return Ok(player);
        }

        [HttpPost]
        public async Task<IActionResult> SavePlayer([FromBody] Player player){
            var playerExists = await repo.PlayerExistsInDb(player.Id);
            if (playerExists ){
                return Conflict();
            }
            var result = repo.SavePlayerToDb(player);
            return CreatedAtAction(nameof(SavePlayer), new {player.Id}, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Player player){
            bool result = await repo.UpdatePlayer(id, player);
            return result ? NoContent() : NotFound();
        }
    }
}