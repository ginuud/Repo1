using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Models.Classes
{
    public class Team
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Player>? Members { get; set; } = [];
    }
}