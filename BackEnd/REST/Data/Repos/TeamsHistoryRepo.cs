using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Models.Classes;
using REST.Data;
using Microsoft.EntityFrameworkCore;

namespace REST.Data.Repos
{
    public class TeamsHistoryRepo
    {
        private readonly DataContext _context;

        public TeamsHistoryRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<TeamHistory> CreateAsync(TeamHistory team){
            await _context.TeamHistory.AddAsync(team);
            await _context.SaveChangesAsync();
            
            return team;
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

        public async Task<TeamHistory?> DeleteAsync(int id, int organizationId) 
        {
            var teamModel = await _context.TeamHistory.Include(m => m.Members).FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (teamModel == null) {
                return null;
            }

            foreach (var player in teamModel.Members) {
                player.TeamId = null;
            }

            _context.TeamHistory.Remove(teamModel);
            await _context.SaveChangesAsync();
            return teamModel;
        }

    }
}