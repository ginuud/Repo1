using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using REST.Data.Repos;
using REST.Dtos.Team;
using REST.Interfaces;
using REST.Mappers;
using REST.Models.Classes;

namespace REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController(TeamsRepo repo) : ControllerBase
    {
        private readonly TeamsRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repo.GetAllAsync();
            var stockDto = result.Select(s => s.ToTeamDto()).ToList();
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id){
            var team = await repo.GetByIdAsync(id);
            if (team == null){
                return NotFound();
            }
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTeam([FromBody] Team team){
            var teamExists = await repo.TeamExists(team.Id);
            if (teamExists ){
                return Conflict();
            }
            var result = repo.CreateAsync(team);
            return CreatedAtAction(nameof(SaveTeam), new {team.Id}, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateTeamRequestDto updateDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var teamModel = await repo.UpdateAsync(id, updateDto);

            if (teamModel == null){
                return NotFound();
            }

            return Ok(teamModel.ToTeamDto());
        }
    }
}