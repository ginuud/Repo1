using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Player;
using REST.Dtos.Team;

namespace REST.Dtos.TeamHistory
{
    public class TeamHistoryDto
    {
        public int Id { get; set;}
        public string? Name { get; set; }= string.Empty;
        public List<PlayerDto> Members { get; set; } = [];
        //public int? GameHistoryId { get; set; }
        public int OrganizationId { get; set; }
    }
}