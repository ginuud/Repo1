using api.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repos
{
    public class PlayersRepo(DataContext context)
    {
        private readonly DataContext context = context;
        public async Task<List<Player>> GetAllPlayers() => await context.Players.ToListAsync();
    }
}