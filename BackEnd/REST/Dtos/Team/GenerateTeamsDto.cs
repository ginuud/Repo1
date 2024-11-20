using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using REST.Dtos.Player;

namespace REST.Dtos.Team
{
    public class GenerateTeamsDto
    {
        [Required]
        public int TeamsCount { get; set;}
        public List<CreateTeamPlayerDto>? Members { get; set; } = [];
        public List<string> TeamNames { get; set; } = [];
        public int OrganizationId { get; set; }
    }
}