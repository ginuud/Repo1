using api.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repos
{
    public class TeamsRepo(DataContext context)
    {
        private readonly DataContext context = context;
        
        public async Task<Team> SaveTeamToDb(Team Team){
            context.Add(Team);
            await context.SaveChangesAsync();
            return Team;
        }
        public async Task<List<Team>> GetAllTeams() => await context.Teams.ToListAsync();
        public async Task<Team?> GetTeamById(int id) => await context.Teams.FindAsync(id);
        public async Task<bool> TeamExistsInDb(int id) => await context.Teams.AnyAsync(x => x.Id == id);
    }
}