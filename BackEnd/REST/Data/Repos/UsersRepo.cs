using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class UsersRepo
    {
        private readonly DataContext _context;
        private IConfiguration _config;

        public UsersRepo(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<string> Login([FromBody] User login)
        {
            var dbUser = await _context.UserList!.FirstOrDefaultAsync(user => user.Username == login.Username);

            if (dbUser == null || dbUser.Password != HashPassword(login.Password)) {
                return "";
            }

            return GenerateJSONWebToken(login);
        }

        private string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: [],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}