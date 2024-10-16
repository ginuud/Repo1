using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using REST.Data.Repos;

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
    }
}