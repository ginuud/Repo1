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
        public string Teams { get; set; } = string.Empty;
        public string? Winner { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
    }
}