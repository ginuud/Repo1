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

        public async Task<List<Team>> CreateMultipleAsync(List<Team> teams)
        {
            await context.Teams.AddRangeAsync(teams);
            await context.SaveChangesAsync();

            return teams;
        }

        public async Task<List<Team>> GetAllAsync(int organizationId)
        {
            var teams = context.Teams.Include(m => m.Members).AsQueryable();

            return await teams.Where(x => x.OrganizationId == organizationId).ToListAsync();
        }
        public async Task<Team?> GetByIdAsync(int id, int organizationId) {
           var dbTeam = await context.Teams
        .Include(m => m.Members)
        .FirstOrDefaultAsync(i => i.Id == id);
        if(dbTeam?.OrganizationId != organizationId){
            return null;
        }
        return dbTeam;
        }

        public async Task<List<Player>> GetPlayersByIdsAsync(List<int> playerIds, int organizationId)
        {
            return await context.Players.Where(m => playerIds.Contains(m.Id)).ToListAsync();
        }

        public async Task<bool> TeamExists(int id) => await context.Teams.AnyAsync(p => p.Id == id);

        public async Task<List<Team>> GenerateTeamsAsync(List<Player> players, List<string> teamNames, int organizationId)
        {
            var sortedPlayers = players.OrderByDescending(p => p.Points).ToList();
            var balancedTeams = new List<Team>();
    
            foreach (var teamName in teamNames)
            {
                balancedTeams.Add(new Team
                {
                    Name = teamName,
                    Members = new List<Player>(),
                    OrganizationId = organizationId
                });
            }

            foreach (var player in sortedPlayers)
            {
                var teamWithLeastPlayersAndPoints = balancedTeams
                    .OrderBy(t => t.Members.Count) 
                    .ThenBy(t => t.Members.Sum(m => m.Points)) 
                    .First();
        
                teamWithLeastPlayersAndPoints.Members.Add(player); 
                Console.WriteLine($"Assigned player {player.Name} to team {teamWithLeastPlayersAndPoints.Name}");
            }

            await CreateMultipleAsync(balancedTeams);

            foreach (var team in balancedTeams)
            {
                Console.WriteLine($"Team {team.Name}: {string.Join(", ", team.Members.Select(m => m.Name))}");
            }

            return balancedTeams; 
        }

        public async Task<Team?> UpdateAsync(int id, UpdateTeamRequestDto teamDto) {           
            
            var existingTeam = await context.Teams.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == teamDto.OrganizationId);

            if (existingTeam == null) {
                return null;
            }
            
            existingTeam.Name = teamDto.Name;

            var membersIds = teamDto.Members.Select(m => m.Id).ToList();
            existingTeam.Members = await context.Players.Where(m => membersIds.Contains(m.Id)).ToListAsync();

            await context.SaveChangesAsync();

            return existingTeam;
        }

        public async Task<Team?> DeleteAsync(int id, int organizationId) 
        {
            var teamModel = await context.Teams.Include(m => m.Members).FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (teamModel == null) {
                return null;
            }

            foreach (var player in teamModel.Members) {
                player.TeamId = null;
            }

            context.Teams.Remove(teamModel);
            await context.SaveChangesAsync();
            return teamModel;
        }
    }
}