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
                TeamId = playerModel.TeamId,
                OrganizationId = playerModel.OrganizationId
            };
        }
        public static Player ToPlayerFromCreate(this CreatePlayerDto playerDto){
            return new Player{
                Name = playerDto.Name,
                Points = playerDto.Points,
                OrganizationId = playerDto.OrganizationId
            };
        }
        public static Player ToPlayerFromUpdate(this UpdatePlayerRequestDto playerDto){
            return new Player{
                Name = playerDto.Name,
                Points = playerDto.Points,
                TeamId = playerDto.TeamId,
                OrganizationId = playerDto.OrganizationId
            };
        }
    }
}