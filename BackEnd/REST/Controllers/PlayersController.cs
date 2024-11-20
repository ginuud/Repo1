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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REST.Controllers
{
    [Authorize]
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
            var organizationId = GetOrganizationId();
            var players = await repo.GetAllAsync(organizationId);
            var playerDto = players.Select(t => t.ToPlayerDto());

            return Ok(playerDto);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPlayer([FromRoute]int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var organizationId = GetOrganizationId();
            var player = await repo.GetByIdAsync(id, organizationId);

            if (player == null) return NotFound();

            return Ok(player);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerDto playerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var organizationId = GetOrganizationId();
            playerDto.OrganizationId = organizationId;
            var playerModel = playerDto.ToPlayerFromCreate();
            await repo.CreateAsync(playerModel);

            return CreatedAtAction(nameof(Create), new {playerModel.Id}, playerModel.ToPlayerDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdatePlayerRequestDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var organizationId = GetOrganizationId();
            updateDto.OrganizationId = organizationId;
            var playerModel = await repo.UpdateAsync(id, updateDto);

            if (playerModel == null) return NotFound();

            return Ok(playerModel.ToPlayerDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete([FromRoute]int id, int organizationId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var playerModel = await repo.DeleteAsync(id, organizationId);

            if (playerModel == null) return NotFound("Player doesn't exist");

            return Ok(playerModel);
        }

        private int GetOrganizationId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return int.Parse(identity!.FindFirst("organizationId")!.Value);
        }
    }
}