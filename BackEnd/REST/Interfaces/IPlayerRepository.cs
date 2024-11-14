using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Player;
using REST.Models.Classes;
using Microsoft.EntityFrameworkCore;


namespace REST.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
        Task<Player> CreateAsync(Player playerModel);
        // public async Task<Player?> UpdateAsync(int id, UpdatePlayerRequestDto playerDto)
        // {
        //     var player = await GetByIdAsync(id);
        //     if (player == null)
        //     {
        //         return null;  
        //     }

        //     player.Name = playerDto.Name ?? player.Name;
        //     player.Points = playerDto.Points;

        //     if (playerDto.TeamId.HasValue)
        //     {
        //         player.TeamId = playerDto.TeamId.Value;
        //     }
        //     await context.SaveChangesAsync();

        //     return player;
        // }
        public async Task<Player?> UpdateAsync(int id, UpdatePlayerRequestDto playerDto)
        {
            var existingPlayer = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);

            if (existingPlayer == null)
            {
                return null;
            }
            
            existingPlayer.Name = playerDto.Name;
            existingPlayer.Points = playerDto.Points;

            if (playerDto.TeamId.HasValue)
            {
                existingPlayer.TeamId = playerDto.TeamId.Value;
            }

            await _context.SaveChangesAsync();

            return existingPlayer;
        }

        Task<Player?> DeleteAsync(int id);
        Task<bool> PlayerExists(int id);
    }
}