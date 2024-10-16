using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(int id);
        Task<Team> CreateAsync(Team stockModel);
        Task<Team?> UpdateAsync(int id, UpdateTeamRequestDto stockDto);
        Task<Team?> DeleteAsync(int id);
        Task<bool> TeamExists(int id);
    }
}