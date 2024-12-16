using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using REST.Dtos.Team;

namespace REST.Dtos.GameHistory
{
    public class CreateGameHistoryDto
    {
         public string? Name { get; set;} = string.Empty;
        public List<TeamDto>? Teams { get; set; }
        public string? Winner { get; set; }
        public int OrganizationId { get; set; }
    }
}