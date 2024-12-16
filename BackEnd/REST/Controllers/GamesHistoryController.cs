using Microsoft.AspNetCore.Mvc;
using REST.Interfaces;
using REST.Mappers;
using REST.Dtos.GameHistory;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using REST.Models.Classes;

namespace REST.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesHistoryController : ControllerBase
    {
        private readonly IGameHistoryRepository repo;
        public GamesHistoryController(IGameHistoryRepository gamesHistoryRepo)
        {
            repo = gamesHistoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var organizationId = GetOrganizationId();
            var games = await repo.GetAllAsync(organizationId);
            return Ok(games);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame([FromRoute] int id){

             var organizationId = GetOrganizationId();
            var game = await repo.GetByIdAsync(id, organizationId);

            if (game == null) return NotFound();

            return Ok(game.ToGameDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGameHistoryDto gameDto){
             if (!ModelState.IsValid) return BadRequest(ModelState);

            var organizationId = GetOrganizationId();
            gameDto.OrganizationId = organizationId;

            // var teamIds = gameDto.Teams?.Select(t => t.Id).ToList() ?? new List<int>();
            // var existingTeams = await repo.GetTeamsByIdsAsync(teamIds);

            // if (existingTeams == null || existingTeams.Count != teamIds.Count)
            // {
            //     return BadRequest("Some teams are invalid or do not exist.");
            // }
            
            var teams = $"{gameDto.Teams[0].Name} vs {gameDto.Teams[1].Name}";
            var gameModel = gameDto.ToGameHistoryFromCreate(teams);
            var result = await repo.CreateAsync(gameModel);

            return CreatedAtAction(nameof(GetGame), new { id = result.Id }, result.ToGameDto());
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateGameRequestDto updateDto){
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //      var organizationId = GetOrganizationId();
        //       updateDto.OrganizationId = organizationId;
        //     var gameModel = await repo.UpdateAsync(id, updateDto);

        //     if (gameModel == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(gameModel.ToGameDto());
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete([FromRoute]int id)
        // {
        //     var organizationId = GetOrganizationId();
        //     var gameModel = await repo.DeleteAsync(id, organizationId);

        //     if (gameModel == null) return NotFound("Game doesn't exist");

        //     return Ok(gameModel.ToGameDto());
        // }

        private int GetOrganizationId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return int.Parse(identity!.FindFirst("organizationId")!.Value);
        }

    }
}