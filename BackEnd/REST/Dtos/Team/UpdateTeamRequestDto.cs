using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using REST.Dtos.Player;

namespace REST.Dtos.Team
{
    public class UpdateTeamRequestDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be over 30 over characters")]
        public string? Name { get; set; }
        public List<CreateTeamPlayerDto>? Members { get; set; } = [];
    }
}