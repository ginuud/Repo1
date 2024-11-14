using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST.Models.Classes
{
    public record Game
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}