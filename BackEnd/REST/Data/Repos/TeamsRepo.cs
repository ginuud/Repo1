using Microsoft.EntityFrameworkCore;
using REST.Dtos.Team;
using REST.Interfaces;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class TeamsRepo(DataContext context) : ITeamRepository
    {
        private readonly DataContext context = context;

        public async Task<Team> CreateAsync(Team team){
            await context.AddAsync(team);
            await context.SaveChangesAsync();
            
            return team;
        }
        public async Task<List<Team>> GetAllAsync()
        {
            var teams = context.Teams.Include(m => m.Members).AsQueryable();

            return await teams.ToListAsync();
        }
        public async Task<Team?> GetByIdAsync(int id) => await context.Teams.Include(m => m.Members).FirstOrDefaultAsync(i => i.Id == id);
        public async Task<bool> TeamExists(int id) => await context.Teams.AnyAsync(p => p.Id == id);

        public async Task<Team?> UpdateAsync(int id, UpdateTeamRequestDto teamDto) {           
            
            var existingTeam = await context.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (existingTeam == null) {
                return null;
            }
            
            existingTeam.Name = teamDto.Name;
            await context.SaveChangesAsync();

            return existingTeam;
        }

        public async Task<Team?> DeleteAsync(int id) {
            Team? teamInDb = await GetByIdAsync(id);
            if (teamInDb == null) {
                return null;
            }

            context.Remove(teamInDb);
            await context.SaveChangesAsync();

            return teamInDb;
        }
    }
}