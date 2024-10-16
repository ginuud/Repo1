using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Dtos.Player
{
    public class CreatePlayerDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Name cannot be over 10 over characters")]
        public string? Name { get; set; }
        [Required]
        [Range(1, 100)]
        public int Points { get; set; }
        public int Rank { get; set; }
        public int TeamId { get; set; }
    }
}