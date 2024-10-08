using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Models.Enums;

namespace REST.Models.Classes
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Team? Team { get; set; }
        public int Points { get; set; }
        public int Rank { get; set; }
    }
}