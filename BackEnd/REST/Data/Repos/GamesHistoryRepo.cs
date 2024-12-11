using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Interfaces;
using REST.Models.Classes;
using REST.Dtos.Game;
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
            var games = context.GameHistory.Include(t => t.Teams).AsQueryable();

            return await games.Where(x => x.OrganizationId == organizationId).ToListAsync();
        }
        public async Task<GameHistory?> GetByIdAsync(int id, int organizationId) {

            var dbGame = await context.GameHistory
                .Include(t => t.Teams)
                .FirstOrDefaultAsync(i => i.Id == id);
            if(dbGame?.OrganizationId != organizationId){
                return null;
            }
            return dbGame;
        } 

        public async Task<List<Team>> GetTeamsByIdsAsync(List<int> teamIds)
        {
            return await context.Teams.Where(t => teamIds.Contains(t.Id)).ToListAsync();
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

        public async Task<GameHistory?> DeleteAsync(int id, int organizationId) {
            var gameModel = await context.GameHistory.Include(g => g.Teams).FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (gameModel == null) {
                return null;
            }

            foreach (var team in gameModel.Teams) {
                team.GameId = null;
            }
            await context.SaveChangesAsync();

            context.GameHistory.Remove(gameModel);
            await context.SaveChangesAsync();
            return gameModel;	
        }        
        
    }
}