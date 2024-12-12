using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.TeamHistory;
using REST.Models.Classes;

namespace REST.Mappers
{
    public static class TeamHistoryMapper
    {
        public static TeamHistoryDto ToTeamHistoryDto(this TeamHistory teamModel){
            return new TeamHistoryDto{
                Id = teamModel.Id,
                Name = teamModel.Name,
                Members = teamModel.Members.Select(c => c.ToPlayerDto()).ToList(),
                GameHistoryId = teamModel.GameHistoryId,
                OrganizationId = teamModel.OrganizationId
            };
        }
        public static TeamHistory ToTeamHistoryFromCreate(this CreateTeamHistoryDto teamDto, List<Player> members){
            return new TeamHistory{
                Name = teamDto.Name,
                Members = members,
                OrganizationId = teamDto.OrganizationId
            };
        }
        
    }
}