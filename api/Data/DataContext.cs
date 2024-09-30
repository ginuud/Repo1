using api.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Player> players{ get; set; }
        public DbSet<Game> games{ get; set; }
    }
}