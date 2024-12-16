using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Player;

namespace REST.Dtos.TeamHistory
{
    public class UpdateTeamHistoryRequestDto
    {
        public string? Name { get; set; }
        public List<CreateTeamPlayerDto>? Members { get; set; } = [];
        public int OrganizationId { get; set; }
        //public int? GameHistoryId { get; set; }
    }
}