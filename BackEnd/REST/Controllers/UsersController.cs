using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using REST.Data.Repos;
using REST.Models.Classes;

namespace REST.Controllers
{ 
    [Route("api/[controller]")]
    public class UsersController(UsersRepo repo) : ControllerBase()
    {
        private readonly UsersRepo repo = repo;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var token = await repo.Login(login);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}