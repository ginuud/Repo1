using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Team;

namespace REST.Dtos.Game
{
    public class GameDto
    {
        public int Id { get; set; }
        public string? Name { get; set;} = string.Empty;
        public List<TeamDto>? Teams { get; set; }
        public int OrganizationId { get; set; }
    }
}