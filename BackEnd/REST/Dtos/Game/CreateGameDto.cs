using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using REST.Dtos.Team;

namespace REST.Dtos.Game
{
    public class CreateGameDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Name cannot be over 10 over characters")]
        public string? Name { get; set; }
        
        [Required]
        //[MaxListLength(5, ErrorMessage = "Teams list cannot contain more than 5 teams.")]
        public List<TeamDto>? Teams { get; set; }
    }
}