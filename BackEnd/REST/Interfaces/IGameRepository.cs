using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Game;
using REST.Models.Classes;

namespace REST.Interfaces
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllAsync(int organizationId);
        Task<Game?> GetByIdAsync(int id, int organizationId);
        Task<List<Team>> GetTeamsByIdsAsync(List<int> teamIds);
        Task<Game> CreateAsync(Game gameModel);       
        Task<Game?> UpdateAsync(int id, UpdateGameRequestDto gameDto);
        Task<Game?> DeleteAsync(int id, int organizationId);
        Task<bool> GameExists(int id);
    }
}