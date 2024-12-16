using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Team;

namespace REST.Dtos.TeamHistory
{
    public class CreateTeamHistoryDto
    {
        public string? Name { get; set; }
        public List<PlayerDto> Members { get; set; } = [];
        //public int? GameHistoryId { get; set; }
        public int OrganizationId { get; set; }
    }
}