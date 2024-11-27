using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using REST.Dtos.Player;

namespace REST.Dtos.Team
{
    public class CreateTeamDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be over 30 over characters")]
        public string? Name { get; set; }
        public List<CreateTeamPlayerDto>? Members { get; set; } = [];
        public int OrganizationId { get; set; }
    }
} 