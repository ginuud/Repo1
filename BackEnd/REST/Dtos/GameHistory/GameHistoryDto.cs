using REST.Dtos.Team;

namespace REST.Dtos.GameHistory
{
    public class GameHistoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set;} = string.Empty;
        public List<TeamDto>? Teams { get; set; }
        public int OrganizationId { get; set; }
    }
}