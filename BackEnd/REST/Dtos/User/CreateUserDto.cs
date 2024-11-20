using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Dtos.User
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot be over 30 over characters")]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}