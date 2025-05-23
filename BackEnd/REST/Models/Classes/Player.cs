using REST.Models.Classes;

namespace REST.Models.Classes
{
    public record Player
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int Points { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public int OrganizationId { get; set; }
    }
}