using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(int id);
        Task<List<Player>> GetPlayersByIdsAsync(List<int> playerIds);
        Task<Team> CreateAsync(Team teamModel);
        Task<Team?> UpdateAsync(int id, UpdateTeamRequestDto stockDto);
        Task<Team?> DeleteAsync(int id);
        Task<bool> TeamExists(int id);
    }
}