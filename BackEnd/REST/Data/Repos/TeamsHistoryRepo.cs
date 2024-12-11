using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class TeamsHistoryRepo
    {
        private readonly DataContext _context;

        public TeamsHistoryRepo( DataContext context)
        {
            _context = context;
        }

        public async Task<TeamHistory> CreateAsync(TeamHistory team){
            await context.TeamHistory.AddAsync(team);
            await context.SaveChangesAsync();
            
            return team;
        }
    }
}