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
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();
            
            return player;
        }

        public async Task<List<Player>> GetAllAsync(int organizationId)
        {
            var players = context.Players.Include(t => t.Team).AsQueryable();

            return await players.Where(x => x.OrganizationId == organizationId).ToListAsync();
        }
        public async Task<Player?> GetByIdAsync(int id, int organizationId){
            var dbPlayer = await context.Players
                .Include(t => t.Team)
                .ThenInclude(m => m.Members)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (dbPlayer?.OrganizationId != organizationId)
            {
                return null;
            }

            return dbPlayer;
        } 
        
        public async Task<bool> PlayerExists(int id) => await context.Players.AnyAsync(p => p.Id == id);

        public async Task<Player?> UpdateAsync(int id, UpdatePlayerRequestDto playerDto) 
        {
            var existingPlayer = await context.Players.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == playerDto.OrganizationId);

            if (existingPlayer == null) {
                return null;
            }
            existingPlayer.Name = playerDto.Name;
            existingPlayer.Points = playerDto.Points;
            existingPlayer.TeamId = playerDto.TeamId;

            await context.SaveChangesAsync();

            return existingPlayer;
        }

        public async Task<Player?> DeleteAsync(int id, int organizationId) 
        {
            var playerModel = await context.Players.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (playerModel == null) {
                return null;
            }

            context.Players.Remove(playerModel);
            await context.SaveChangesAsync();
            return playerModel;	
        }
    }
}
