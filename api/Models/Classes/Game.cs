
namespace api.Models.Classes
{
    public class Game
    {
        public int Id { get; set;}
        public string Name { get; set; } = "";
        public List<Player> Players { get; set; } = [];
    }
}