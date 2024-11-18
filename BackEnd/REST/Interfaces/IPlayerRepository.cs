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
        Task<Player?> UpdateAsync(int id, UpdatePlayerRequestDto playerDto);
        Task<Player?> DeleteAsync(int id);
        Task<bool> PlayerExists(int id);
    }
}