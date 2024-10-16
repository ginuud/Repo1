using Microsoft.EntityFrameworkCore;
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
        public static Team ToTeamFromCreate(this CreateTeamDto teamDto, int playerId, List<Player> members){
            return new Team{
                Name = teamDto.Name,
                Members = members
            };
        }
        public static Team ToTeamFromUpdate(this UpdateTeamRequestDto teamDto){
            return new Team{
                Name = teamDto.Name,
                //Members = teamDto.Members.
            };
        }
    }
}