using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Dtos.Player
{
    public class UpdatePlayerRequestDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be over 30 over characters")]
        public string? Name { get; set; }
        [Required]
        [Range(0, 10000000)]
        public int Points { get; set; }
        public int? TeamId { get; set; }
        public int OrganizationId { get; set; }
    }
}
