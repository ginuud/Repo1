using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Models.Classes
{
    public class TeamHistory
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public List<Player>? Members { get; set; } = [];
        public int? GameHistoryId { get; set; }
        public GameHistory? GameHistory { get; set; } 
        public int OrganizationId { get; set; }

    }
}