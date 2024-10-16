using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Mappers
{
    public static class TeamMapper
    {
        public static TeamDto ToTeamDto(this Team teamModel){
            return new TeamDto{
                Id = teamModel.Id,
                Name = teamModel.Name,
                Members = teamModel.Members.Select(c => c.ToPlayerDto()).ToList(),
            };
        }
    }
}