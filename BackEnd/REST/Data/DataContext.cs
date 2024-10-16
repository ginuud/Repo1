using Microsoft.EntityFrameworkCore;
using REST.Models.Classes;
using REST.Models.Enums;

namespace REST.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Player>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Player>().Property(p => p.Id).HasIdentityOptions(startValue: 4);

            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    Id = 1,
                    Name = "Michael Jordan",
                    Team = Team.Bulls,
                    Points = 32292,
                    Rank = 1
                },
                new Player
                {
                    Id = 2,
                    Name = "LeBron James",
                    Team = Team.Lakers,
                    Points = 35367,
                    Rank = 2
                },
                new Player
                {
                    Id = 3,
                    Name = "Kobe Bryant",
                    Team = Team.Lakers,
                    Points = 33643,
                    Rank = 3
                },
                new Player
                {
                    Id = 4,
                    Name = "Magic Johnson",
                    Team = Team.Lakers,
                    Points = 17707,
                    Rank = 4
                },
                new Player
                {
                    Id = 5,
                    Name = "Larry Bird",
                    Team = Team.Celtics,
                    Points = 21791,
                    Rank = 5
                }
            );
        }
    }
}