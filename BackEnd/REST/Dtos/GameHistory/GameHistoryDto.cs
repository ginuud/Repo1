using REST.Dtos.Team;
using REST.Dtos.TeamHistory;

namespace REST.Dtos.GameHistory
{
    public class GameHistoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set;} = string.Empty;
        public string Teams { get; set; } = string.Empty;      
        public string? Winner { get; set; }
        public int OrganizationId { get; set; }
    }
}