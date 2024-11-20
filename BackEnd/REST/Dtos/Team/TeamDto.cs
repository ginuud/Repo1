using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Models.Classes;

namespace REST.Dtos.Team
{
    public class TeamDto
    {
        public int Id { get; set;}
        public string? Name { get; set; }
        public List<PlayerDto> Members { get; set; } = [];
        
    }
}