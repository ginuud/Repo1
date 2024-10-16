using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Dtos.Team
{
    public class PlayerDto
    {
        public string? Name { get; set; }
        public int Points { get; set; }
        public int Rank { get; set; }
        public int TeamId { get; set; }
        //public Team? Team { get; set; }
    }
}