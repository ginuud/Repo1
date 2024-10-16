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
    public class TeamsController(TeamsRepo repo) : ControllerBase
    {
        private readonly TeamsRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repo.GetAllTeams();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id){
            var team = await repo.GetTeamById(id);
            if (team == null){
                return NotFound();
            }
            return Ok(team);
        }
        [HttpPost]
        public async Task<IActionResult> SaveTeam([FromBody] Team team){
            var teamExists = await repo.TeamExistsInDb(team.Id);
            if (teamExists ){
                return Conflict();
            }
            var result = repo.SaveTeamToDb(team);
            return CreatedAtAction(nameof(SaveTeam), new {team.Id}, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Team team){
            bool result = await repo.UpdateTeam(id, team);
            return result ? NoContent() : NotFound();
        }
    }
}