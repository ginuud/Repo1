using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.GameHistory;
using REST.Models.Classes;

namespace REST.Interfaces
{
    public interface IGameHistoryRepository
    {
        Task<List<GameHistory>> GetAllAsync(int organizationId);
        Task<GameHistory?> GetByIdAsync(int id, int organizationId);
        Task<List<Team>> GetTeamsByIdsAsync(List<int> teamIds);
        Task<GameHistory> CreateAsync(GameHistory gameModel);       
        //Task<GameHistory?> UpdateAsync(int id, UpdateGameRequestDto gameDto);
        //Task<GameHistory?> DeleteAsync(int id, int organizationId);
        Task<bool> GameExists(int id);
    }
}