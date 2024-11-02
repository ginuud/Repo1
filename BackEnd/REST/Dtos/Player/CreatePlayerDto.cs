using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Dtos.Player
{
    public class CreatePlayerDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be over 30 over characters")]
        public string? Name { get; set; }
        [Required]
        public int Points { get; set; }
        public int Rank { get; set; }
    }
}