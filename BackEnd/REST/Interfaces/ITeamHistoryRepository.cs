using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.TeamHistory;
using REST.Models.Classes;

namespace REST.Interfaces
{
    public interface ITeamHistoryRepository
    {
        Task<List<TeamHistory>> GetAllAsync(int organizationId);
        Task<TeamHistory?> GetByIdAsync(int id, int organizationId);
        Task<List<Player>> GetPlayersByIdsAsync(List<int> playerIds, int organizationId);
        Task<TeamHistory> CreateAsync(TeamHistory teamModel);
        Task<List<TeamHistory>> CreateMultipleAsync(List<TeamHistory> teams);
        Task<List<TeamHistory>> GenerateTeamsHistoryAsync(List<Player> players, List<string> teamNames, int organizationId);
        Task<TeamHistory?> UpdateAsync(int id, UpdateTeamHistoryRequestDto teamDto);
        Task<TeamHistory?> DeleteAsync(int id, int organizationId);
        Task<bool> TeamHistoryExists(int id);
    }
}