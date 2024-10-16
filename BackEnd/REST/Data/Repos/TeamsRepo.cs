using Microsoft.EntityFrameworkCore;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class TeamsRepo(DataContext context)
    {
        private readonly DataContext context = context;

        public async Task<Team> SaveTeamToDb(Team team){
            context.Add(team);
            await context.SaveChangesAsync();
            return team;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            IQueryable<Team> query = context.Teams.AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<Team?> GetTeamById(int id) => await context.Teams.FindAsync(id);
        public async Task<bool> TeamExistsInDb(int id) => await context.Teams.AnyAsync(p => p.Id == id);

        public async Task<bool> UpdateTeam(int id, Team team) {
            bool idMatch = id == team.Id;
            bool teamExists = await TeamExistsInDb(id);

            if (!idMatch || !teamExists) {
                return false;
            }
            context.Update(team);
            int updatedRecordsCount = await context.SaveChangesAsync();
            return updatedRecordsCount == 1;
        }

        public async Task<bool> DeleteTeamById(int id) {
            Team? teamInDb = await GetTeamById(id);
            if (teamInDb == null) {
                return false;
            }
            context.Remove(teamInDb);
            int changesCount = await context.SaveChangesAsync();
            return changesCount == 1;
        }
    }
}