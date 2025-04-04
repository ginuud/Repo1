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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REST.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository repo;

        public TeamsController(ITeamRepository teamRepo)
        {
            repo = teamRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizationId = GetOrganizationId();
            var result = await repo.GetAllAsync(organizationId);
            var teamDto = result.Select(s => s.ToTeamDto()).ToList();

            return Ok(teamDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTeam(int id)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var organizationId = GetOrganizationId();
            var team = await repo.GetByIdAsync(id, organizationId);

            if (team == null) return NotFound();

            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto teamDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var organizationId = GetOrganizationId();
            teamDto.OrganizationId = organizationId;
            var playerIDs = teamDto.Members.Select(m => m.Id).ToList();
            var existingPlayers = await repo.GetPlayersByIdsAsync(playerIDs, organizationId);

            if (existingPlayers == null || existingPlayers.Count != playerIDs.Count)
            {
                return BadRequest("Some players are invalid or do not exist.");
            }

            var teamModel = teamDto.ToTeamFromCreate(existingPlayers);
            await repo.CreateAsync(teamModel);

            return CreatedAtAction(nameof(GetTeam), new {id = teamModel.Id}, teamModel.ToTeamDto());
        }

        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GenerateTeams([FromBody] GenerateTeamsDto teamsDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var organizationId = GetOrganizationId();
            teamsDto.OrganizationId = organizationId;
            var playerIDs = teamsDto.Members.Select(m => m.Id).ToList();
            var existingPlayers = await repo.GetPlayersByIdsAsync(playerIDs, organizationId);

            if (existingPlayers == null || existingPlayers.Count != playerIDs.Count)
            {
                return BadRequest("Some players are invalid or do not exist.");
            }

            var generatedTeams = await repo.GenerateTeamsAsync(existingPlayers, teamsDto.TeamNames, organizationId);

            var generatedTeamDtos = generatedTeams.Select(t => t.ToTeamDto()).ToList();
            return Ok(generatedTeamDtos);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateTeamRequestDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var organizationId = GetOrganizationId();
            updateDto.OrganizationId = organizationId;  
            var teamModel = await repo.UpdateAsync(id, updateDto);

            if (teamModel == null) return NotFound();

            return Ok(teamModel.ToTeamDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var organizationId = GetOrganizationId();
            var teamModel = await repo.DeleteAsync(id, organizationId);

            if (teamModel == null) return NotFound("Team doesn't exist");

            return Ok(teamModel);
        }

         private int GetOrganizationId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return int.Parse(identity!.FindFirst("organizationId")!.Value);
        }
    }
}