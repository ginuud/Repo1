using REST.Dtos.Player;
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
        public static Player ToPlayerFromCreate(this CreatePlayerDto playerDto, int teamId){
            return new Player{
                Name = playerDto.Name,
                Points = playerDto.Points,
                TeamId = teamId,
            };
        }
        public static Player ToPlayerFromUpdate(this UpdatePlayerRequestDto playerDto, int teamId){
            return new Player{
                Name = playerDto.Name,
                Points = playerDto.Points,
                TeamId = teamId,
            };
        }
    }
}