using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.GameHistory;
using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Mappers
{
    
 public static class GameHistoryMapper
    {
        public static GameHistoryDto ToGameDto(this GameHistory gameHistoryModel)
        {
            return new GameHistoryDto
            {
                Id = gameHistoryModel.Id,
                Name = gameHistoryModel.Name,
                //Teams = gameHistoryModel.Teams,
                Winner = gameHistoryModel.Winner,
                OrganizationId = gameHistoryModel.OrganizationId
            };
        }

        public static GameHistory ToGameHistoryFromCreate(this CreateGameHistoryDto createDto, string teams)
        {
            return new GameHistory
            {
                Name = createDto.Name,
                Teams = teams,
                Winner = createDto.Winner,
                OrganizationId = createDto.OrganizationId
            };
        }
    }
}