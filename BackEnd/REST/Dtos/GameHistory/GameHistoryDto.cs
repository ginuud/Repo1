using REST.Dtos.Team;
using REST.Dtos.TeamHistory;

namespace REST.Dtos.GameHistory
{
    public class GameHistoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set;} = string.Empty;
        public List<TeamHistoryDto> TeamHistories { get; set; } = new();         
        public string? Winner { get; set; }
        public int OrganizationId { get; set; }
    }
}