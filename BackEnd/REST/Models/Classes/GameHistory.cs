using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Models.Classes
{
    public class GameHistory
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public List<TeamHistory>? TeamHistories { get; set; } = new();
        public string? Winner { get; set; } = string.Empty;
        public Game? Game { get; set; }
        public int OrganizationId { get; set; }
    }
}