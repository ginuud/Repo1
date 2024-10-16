using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.Replication;
using REST.Dtos.Team;
using REST.Models.Classes;

namespace REST.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerDto ToPlayerDto(this Player playerModel){
            return new PlayerDto{
                Id = playerModel.Id,
                Name = playerModel.Name,
                Points = playerModel.Points,
                Rank = playerModel.Rank,
                TeamId = playerModel.TeamId,
            };
        }
    }
}