using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.Replication;
using REST.Dtos.Player;
using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerDto ToPlayerDto(this Player playerModel){
            return new PlayerDto{
                Name = playerModel.Name,
                Points = playerModel.Points,
                Rank = playerModel.Rank,
                TeamId = playerModel.TeamId,
            };
        }
        public static Player ToPlayerFromCreate(this CreatePlayerDto playerDto){
            return new Player{
                Name = playerDto.Name,
                Points = playerDto.Points,
                TeamId = playerDto.TeamId,
            };
        }
        public static Player ToPlayerFromUpdate(this UpdatePlayerRequestDto playerDto, int playerId){
            return new Player{
                Name = playerDto.Name,
                Points = playerDto.Points,
                TeamId = playerDto.TeamId,
            };
        }
    }
}