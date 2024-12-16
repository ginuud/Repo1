using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Interfaces;
using REST.Models.Classes;
using REST.Dtos.GameHistory;
using Microsoft.EntityFrameworkCore;

namespace REST.Data.Repos
{
    public class GamesHistoryRepo (DataContext context) : IGameHistoryRepository
    {
        private readonly DataContext context = context;

        public async Task<GameHistory> CreateAsync(GameHistory game){
            await context.GameHistory.AddAsync(game);
            await context.SaveChangesAsync();
            return game;
        }

        public async Task<List<GameHistory>> GetAllAsync(int organizationId)
        {
           return await context.GameHistory.Where(g => g.OrganizationId == organizationId).ToListAsync();
        }
        public async Task<GameHistory?> GetByIdAsync(int id, int organizationId) {

            return await context.GameHistory
                .FirstOrDefaultAsync(g => g.Id == id && g.OrganizationId == organizationId);
        } 

        public async Task<List<Team>> GetTeamsByIdsAsync(List<int> teamIds)
        {
            return await context.Teams
                .Where(t => teamIds.Contains(t.Id))
                .ToListAsync();
        }
        
        public async Task<bool> GameExists(int id) => await context.GameHistory.AnyAsync(p => p.Id == id);

        // public async Task<Game?> UpdateAsync(int id, UpdateGameRequestDto gameDto) {
            
        //     var existingGame = await context.Games
        //     .Include(g => g.Teams)
        //     .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == gameDto.OrganizationId);

        //     if (existingGame == null) {
        //         return null;
        //     }
        //     existingGame.Name = gameDto.Name;

        //     if (gameDto.TeamIds != null && gameDto.TeamIds.Any()){
        //         existingGame.Teams.Clear();
        //         var teams = await context.Teams.Where(t => gameDto.TeamIds.Contains(t.Id)).ToListAsync();
        //         foreach (var team in teams)
        //         {
        //             existingGame.Teams.Add(team);
        //         }
        //     }
        //     await context.SaveChangesAsync();
        //     return existingGame;
        // }

        // public async Task<GameHistory?> DeleteAsync(int id, int organizationId) {
        //     var gameModel = await context.GameHistory
        //         .FirstOrDefaultAsync(g => g.Id == id && g.OrganizationId == organizationId);

        //     if (gameModel == null) return null;

        //     context.Games.Remove(gameModel);
        //     await context.SaveChangesAsync();

        //     return gameModel;
	
        // }        
        
    }
}