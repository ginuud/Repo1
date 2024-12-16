using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Models.Classes;
using REST.Data;
using Microsoft.EntityFrameworkCore;
using REST.Dtos.TeamHistory;
using REST.Interfaces;

namespace REST.Data.Repos
{
    public class TeamsHistoryRepo : ITeamHistoryRepository
    {
        private readonly DataContext _context;

        public TeamsHistoryRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<TeamHistory> CreateAsync(TeamHistory teamModel){
            await _context.TeamHistory.AddAsync(teamModel);
            await _context.SaveChangesAsync();
            
            return teamModel;
        }
        public async Task<List<TeamHistory>> CreateMultipleAsync(List<TeamHistory> teams)
        {
            await _context.TeamHistory.AddRangeAsync(teams);
            await _context.SaveChangesAsync();

            return teams;
        }
        public async Task<List<TeamHistory>> GetAllAsync(int organizationId)
        {
            var teams = _context.TeamHistory.Include(m => m.Members).AsQueryable();

            return await teams.Where(x => x.OrganizationId == organizationId).ToListAsync();
        }
        public async Task<TeamHistory?> GetByIdAsync(int id, int organizationId) {
           var dbTeam = await _context.TeamHistory
        .Include(m => m.Members)
        .FirstOrDefaultAsync(i => i.Id == id);
        if(dbTeam?.OrganizationId != organizationId){
            return null;
        }
        return dbTeam;
        }

        public async Task<List<Player>> GetPlayersByIdsAsync(List<int> playerIds, int organizationId)
        {
            return await _context.Players.Where(m => playerIds.Contains(m.Id)).ToListAsync();
        }

        public async Task<bool> TeamExists(int id) => await _context.TeamHistory.AnyAsync(p => p.Id == id);

        public async Task<Team?> UpdateAsync(int id, UpdateTeamHistoryRequestDto teamDto, int organizationId) {           
            
           var team = await _context.TeamHistory
                .Include(m => m.Members)
                .FirstOrDefaultAsync(th => th.Id == id && th.OrganizationId == organizationId);

            if (team == null) return null;

            team.Name = teamDto.Name;
            team.Members = await _context.Players
                .Where(p => teamDto.Members.Select(m => m.Id).Contains(p.Id))
                .ToListAsync();

            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<TeamHistory?> DeleteAsync(int id, int organizationId) 
        {
            var team = await GetByIdAsync(id, organizationId);

            if (team == null) return null;

            _context.TeamHistory.Remove(team);
            await _context.SaveChangesAsync();
            return team;
        }

    }
}