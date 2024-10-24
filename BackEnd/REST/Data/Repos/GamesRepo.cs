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
    public class GamesRepo (DataContext context) : IGameRepository
    {
        private readonly DataContext context = context;

        public async Task<Game> CreateAsync(Game game){
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();
            return game;
        }

        public async Task<List<Game>> GetAllAsync()
        {
            var games = context.Games.Include(t => t.Teams).AsQueryable();

            return await games.ToListAsync();
        }
        public async Task<Game?> GetByIdAsync(int id) => await context.Games
        .Include(t => t.Teams)
        .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<List<Team>> GetTeamsByIdsAsync(List<int> teamIds)
        {
            return await context.Teams.Where(t => teamIds.Contains(t.Id)).ToListAsync();
        }
        
        public async Task<bool> GameExists(int id) => await context.Games.AnyAsync(p => p.Id == id);

        public async Task<Game?> UpdateAsync(int id, UpdateGameRequestDto gameDto) {
            
            var existingGame = await context.Games.FirstOrDefaultAsync(x => x.Id == id);

            if (existingGame == null) {
                return null;
            }
            existingGame.Name = gameDto.Name;

            await context.SaveChangesAsync();

            return existingGame;
        }

        public async Task<Game?> DeleteAsync(int id) {
            var gameModel = await context.Games.FirstOrDefaultAsync(x => x.Id == id);

            if (gameModel == null) {
                return null;
            }

            context.Games.Remove(gameModel);
            await context.SaveChangesAsync();
            return gameModel;	
        }
        
    }
}