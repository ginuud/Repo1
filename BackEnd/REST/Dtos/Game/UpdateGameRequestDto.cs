using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace REST.Dtos.Game
{
    public class UpdateGameRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Name cannot be over 10 over characters")]
        public string? Name { get; set; }
        
        [Required]
        public List<int>? TeamIds { get; set; }
        
    }
}