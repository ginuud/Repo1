using Microsoft.AspNetCore.Mvc;
using REST.Interfaces;
using REST.Mappers;
using REST.Dtos.Game;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REST.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository repo;
        public GamesController(IGameRepository gamesRepo)
        {
            repo = gamesRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizationId = GetOrganizationId();
            var games = await repo.GetAllAsync(organizationId);
            var gameDto = games.Select(t => t.ToGameDto());
            return Ok(gameDto);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame([FromRoute] int id){

            var organizationId = GetOrganizationId();
            var game = await repo.GetByIdAsync(id, organizationId);
        
            if (game == null){
                return NotFound();
            }
            return Ok(game.ToGameDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGameDto gameDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var organizationId = GetOrganizationId();
            gameDto.OrganizationId = organizationId;
            var teamIDs = gameDto.Teams.Select(t => t.Id).ToList();
            var existingTeams = await repo.GetTeamsByIdsAsync(teamIDs);

            if (existingTeams == null || existingTeams.Count != teamIDs.Count)
            {
                return BadRequest("Some teams are invalid or do not exist.");
            }

            var gameModel = gameDto.ToGameFromCreate(existingTeams); 
            var result = await repo.CreateAsync(gameModel);
            return CreatedAtAction(nameof(GetGame), new {id = gameModel.Id}, gameModel.ToGameDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateGameRequestDto updateDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

             var organizationId = GetOrganizationId();
              updateDto.OrganizationId = organizationId;
            var gameModel = await repo.UpdateAsync(id, updateDto);

            if (gameModel == null)
            {
                return NotFound();
            }

            return Ok(gameModel.ToGameDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var organizationId = GetOrganizationId();
            var gameModel = await repo.DeleteAsync(id, organizationId);

            if (gameModel == null) return NotFound("Game doesn't exist");

            return Ok(gameModel);
        }

        private int GetOrganizationId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return int.Parse(identity!.FindFirst("organizationId")!.Value);
        }

    }
}