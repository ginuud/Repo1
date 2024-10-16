using REST.Models.Classes;

namespace REST.Models.Classes
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Points { get; set; }
        public int Rank { get; set; }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
    }
}