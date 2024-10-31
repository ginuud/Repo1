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
            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();
            
            return team;
        }
        public async Task<List<Team>> GetAllAsync()
        {
            var teams = context.Teams.Include(m => m.Members).AsQueryable();

            return await teams.ToListAsync();
        }
        public async Task<Team?> GetByIdAsync(int id) => await context.Teams
        .Include(m => m.Members)
        .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<List<Player>> GetPlayersByIdsAsync(List<int> playerIds)
        {
            return await context.Players.Where(m => playerIds.Contains(m.Id)).ToListAsync();
        }

        public async Task<bool> TeamExists(int id) => await context.Teams.AnyAsync(p => p.Id == id);

        public async Task<Team?> UpdateAsync(int id, UpdateTeamRequestDto teamDto) {           
            
            var existingTeam = await context.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (existingTeam == null) {
                return null;
            }
            
            existingTeam.Name = teamDto.Name;

            var membersIds = teamDto.Members.Select(m => m.Id).ToList();
            existingTeam.Members = await context.Players.Where(m => membersIds.Contains(m.Id)).ToListAsync();

            await context.SaveChangesAsync();

            return existingTeam;
        }

        public async Task<Team?> DeleteAsync(int id) 
        {
            var teamModel = await context.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (teamModel == null) {
                return null;
            }

            context.Teams.Remove(teamModel);
            await context.SaveChangesAsync();
            return teamModel;
        }
    }
}