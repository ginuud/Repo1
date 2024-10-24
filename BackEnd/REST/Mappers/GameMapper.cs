using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Game;
using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Mappers
{
    public static class GameMapper
    {
        public static GameDto ToGameDto(this Game gameModel){
            return new GameDto{
                Id = gameModel.Id,
                Name = gameModel.Name,
                Teams = gameModel.Teams.Select(t => t.ToTeamDto()).ToList()       
            };
        }
        public static Game ToGameFromCreate(this CreateGameDto gameDto, List<Team> teams){
            return new Game{
                Name = gameDto.Name,
                Teams = teams
            };
        }
        public static Game ToGameFromUpdate(this UpdateGameRequestDto gameDto){
            return new Game{
                Name = gameDto.Name                
            };
        }
    }
}