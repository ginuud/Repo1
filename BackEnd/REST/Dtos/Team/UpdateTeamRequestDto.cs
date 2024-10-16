using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Dtos.Team
{
    public class UpdateTeamRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Name cannot be over 10 over characters")]
        public string? Name { get; set; }
        public List<PlayerDto>? Members { get; set; } = [];
    }
}