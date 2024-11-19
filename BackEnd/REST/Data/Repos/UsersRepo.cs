using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> Login([FromBody] User login)
        {
            var dbUser = await _context.UserList!.FirstOrDefaultAsync(user => user.Username == login.Username);

            return dbUser != null && dbUser.Password == HashPassword(login.Password);
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
    }
}