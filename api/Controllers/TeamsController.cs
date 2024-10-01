using api.Data.Repos;
using api.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController(TeamsRepo repo) : ControllerBase()
    {
        private readonly TeamsRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var result = await repo.GetAllTeams();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTeam([FromBody] Team team){
            var teamExists = await repo.TeamExistsInDb(team.Id);
            if (teamExists){
                return Conflict();
            }
            var result = repo.SaveTeamToDb(team);
            return CreatedAtAction(nameof(SaveTeam), new {team.Id}, result);
        }
    }
}