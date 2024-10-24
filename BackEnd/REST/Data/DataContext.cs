using Microsoft.EntityFrameworkCore;
using REST.Models.Classes;

namespace REST.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(p => p.TeamId);

            modelBuilder.Entity<Player>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Player>().Property(p => p.Id).HasIdentityOptions(startValue: 4);

            modelBuilder.Entity<Team>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Team>().Property(p => p.Id).HasIdentityOptions(startValue: 6);

            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    Id = 1,
                    Name = "Bulls",
                },
                new Team
                {
                    Id = 2,
                    Name = "Lakers",
                },
                new Team
                {
                    Id = 3,
                    Name = "Celtics",
                }
            );
            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    Id = 1,
                    Name = "Michael Jordan",
                    TeamId = 1,
                    Points = 32292,
                    Rank = 1
                },
                new Player
                {
                    Id = 2,
                    Name = "LeBron James",
                    TeamId = 2,
                    Points = 35367,
                    Rank = 2
                },
                new Player
                {
                    Id = 3,
                    Name = "Kobe Bryant",
                    TeamId = 2,
                    Points = 33643,
                    Rank = 3
                },
                new Player
                {
                    Id = 4,
                    Name = "Magic Johnson",
                    TeamId = 2,
                    Points = 17707,
                    Rank = 4
                },
                new Player
                {
                    Id = 5,
                    Name = "Larry Bird",
                    TeamId = 3,
                    Points = 21791,
                    Rank = 53
                }
            );
        }
    }
}