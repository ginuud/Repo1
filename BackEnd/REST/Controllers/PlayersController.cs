using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using REST.Data.Repos;
using REST.Dtos.Player;
using REST.Interfaces;
using REST.Mappers;
using REST.Models.Classes;

namespace REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepository repo;



        public PlayersController(IPlayerRepository playersRepo)
        {
            repo = playersRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repo.GetAllAsync();
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id){
            var player = await repo.GetByIdAsync(id);
            if (player == null){
                return NotFound();
            }
            return Ok(player);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Player player){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await repo.CreateAsync(player);
            return CreatedAtAction(nameof(Create), new {player.Id}, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdatePlayerRequestDto updateDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playerModel = await repo.UpdateAsync(id, updateDto);

            if (playerModel == null)
            {
                return NotFound();
            }

            return Ok(playerModel.ToPlayerDto());
        }
    }
}