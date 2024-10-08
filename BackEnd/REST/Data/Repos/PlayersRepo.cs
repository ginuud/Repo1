using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class PlayersRepo(DataContext context)
    {
        private readonly DataContext context = context;

        public async Task<List<Player>> GetAllPlayers()
        {
            IQueryable<Player> query = context.Players.AsQueryable();

            return await query.ToListAsync();
        }
    }
}