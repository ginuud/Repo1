using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class PlayersRepo(DataContext context)
    {
        private readonly DataContext context = context;

        public async Task<Player> SavePlayerToDb(Player player){
            context.Add(player);
            await context.SaveChangesAsync();
            return player;
        }

        public async Task<List<Player>> GetAllPlayers()
        {
            IQueryable<Player> query = context.Players.AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<Player?> GetPlayerById(int id) => await context.Players.FindAsync(id);
        public async Task<bool> PlayerExistsInDb(int id) => await context.Players.AnyAsync(p => p.Id == id);

        public async Task<bool> UpdatePlayer(int id, Player player) {
            bool idMatch = id == player.Id;
            bool playerExists = await PlayerExistsInDb(id);

            if (!idMatch || !playerExists) {
                return false;
            }
            context.Update(player);
            int updatedRecordsCount = await context.SaveChangesAsync();
            return updatedRecordsCount == 1;
        }

        public async Task<bool> DeletePlayerById(int id) {
            Player? playerInDb = await GetPlayerById(id);
            if (playerInDb == null) {
                return false;
            }
            context.Remove(playerInDb);
            int changesCount = await context.SaveChangesAsync();
            return changesCount == 1;
        }
    }
}