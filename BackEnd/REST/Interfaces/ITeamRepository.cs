using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync(int organizationId);
        Task<Team?> GetByIdAsync(int id, int organizationId);
        Task<List<Player>> GetPlayersByIdsAsync(List<int> playerIds, int organizationId);
        Task<Team> CreateAsync(Team teamModel);
        Task<List<Team>> CreateMultipleAsync(List<Team> teams);
        Task<List<Team>> GenerateTeamsAsync(List<Player> players, List<string> teamNames);
        Task<Team?> UpdateAsync(int id, UpdateTeamRequestDto teamDto);
        Task<Team?> DeleteAsync(int id, int organizationId);
        Task<bool> TeamExists(int id);
    }
}