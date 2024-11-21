using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;
using REST.Dtos.Team;
using REST.Models.Classes;
using REST.Mappers;

namespace REST.Mappers
{
    public static class TeamMapper
    {
        public static TeamDto ToTeamDto(this Team teamModel){
            return new TeamDto{
                Id = teamModel.Id,
                Name = teamModel.Name,
                Members = teamModel.Members.Select(c => c.ToPlayerDto()).ToList(),
                GameId = teamModel.GameId
            };
        }
        public static Team ToTeamFromCreate(this CreateTeamDto teamDto, List<Player> members){
            return new Team{
                Name = teamDto.Name,
                Members = members
            };
        }
        // public static Team ToTeamFromUpdate(this UpdateTeamRequestDto teamDto){
        //     return new Team{
        //         Name = teamDto.Name,
        //         Members = teamDto.Members
        //             .Select(member => member.Id)
        //             .ToList()
        //     };
        // }
        // public static Player PlayerToTeamFromUpdate(int teamId){
        //     return new Player{
        //         Id = teamId
        //     };
        // }
    }
}