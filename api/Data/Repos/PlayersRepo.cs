using api.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repos
{
    public class PlayersRepo(DataContext context)
    {
        private readonly DataContext context = context;
        
        public async Task<Player> SavePlayerToDb( Player player){
            context.Add(player);
            await context.SaveChangesAsync();
            return player;
        }
        public async Task<List<Player>> GetAllPlayers() => await context.Players.ToListAsync();
        public async Task<Player?> GetPlayerById(int id) => await context.Players.FindAsync(id);
        public async Task<bool> PlayerExistsInDb(int id) => await context.Players.AnyAsync(x => x.Id == id);
    }
}