using Microsoft.EntityFrameworkCore;
using REST.Dtos.Player;
using REST.Interfaces;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class PlayersRepo(DataContext context) : IPlayerRepository
    {
        private readonly DataContext context = context;

        public async Task<Player> CreateAsync(Player player){
            context.Players.Add(player);
            await context.SaveChangesAsync();
            return player;
        }

        public async Task<List<Player>> GetAllAsync()
        {
            var players = context.Players.Include(t => t.Team).AsQueryable();

            return await players.ToListAsync();
        }
        public async Task<Player?> GetByIdAsync(int id) => await context.Players
        .Include(t => t.Team)
        .ThenInclude(m => m.Members)
        .FirstOrDefaultAsync(i => i.Id == id);
        
        public async Task<bool> PlayerExists(int id) => await context.Players.AnyAsync(p => p.Id == id);

        public async Task<Player?> UpdateAsync(int id, UpdatePlayerRequestDto playerDto) {
            
            var existingPlayer = await context.Players.FirstOrDefaultAsync(x => x.Id == id);

            if (existingPlayer == null) {
                return null;
            }
            existingPlayer.Name = playerDto.Name;
            existingPlayer.Points = playerDto.Points;

            await context.SaveChangesAsync();

            return existingPlayer;
        }

        public async Task<Player?> DeleteAsync(int id) {
            Player? playerInDb = await GetByIdAsync(id);
            if (playerInDb == null) {
                return null;
            }
            context.Remove(playerInDb);
            await context.SaveChangesAsync();
            return playerInDb;	
        }
    }
}