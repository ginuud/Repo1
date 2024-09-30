
namespace api.Models.Classes
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Points { get; set; }
        public int GameId { get; set; }
        public Game? Game { get; set; }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
    }
}