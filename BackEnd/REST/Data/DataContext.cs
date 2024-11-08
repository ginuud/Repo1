using Microsoft.EntityFrameworkCore;
using REST.Models.Classes;

namespace REST.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(p => p.TeamId)
                .IsRequired(false);
            
            modelBuilder.Entity<Game>()
                 .HasMany(g => g.Teams)
                 .WithOne(t => t.Game)
                 .HasForeignKey(t => t.GameId);

            modelBuilder.Entity<Player>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Player>().Property(p => p.Id).HasIdentityOptions(startValue: 8);

            modelBuilder.Entity<Team>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Team>().Property(p => p.Id).HasIdentityOptions(startValue: 6);

            modelBuilder.Entity<Game>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Game>().Property(p => p.Id).HasIdentityOptions(startValue: 3);

            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Name = "Bulls vs Lakers"
                },
                new Game
                {
                    Id = 2,
                    Name = "Celtics vs Spurs"
                }
                // new Game
                // {
                //     Id = 3,
                //     Name = "Wariors"
                // }                 
            );

            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    Id = 1,
                    Name = "Bulls",
                    GameId = 1
                },
                new Team
                {
                    Id = 2,
                    Name = "Lakers",
                    GameId = 1
                },
                new Team
                {
                    Id = 3,
                    Name = "Celtics",
                    GameId =2
                },
                new Team
                {
                    Id = 4,
                    Name = "Spurs",
                    GameId = 2
                }
                // new Team
                // {
                //     Id = 5,
                //     Name = "Warriors",
                //     GameId = 3
                // }

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
                },
                new Player
                {
                    Id = 6,
                    Name = "Kawhi Leonard",
                    TeamId = 4,
                    Points = 13937,
                    Rank = 27
                },
                new Player
                {
                    Id = 7,
                    Name = "Victor Wembanyama",
                    TeamId = 4,
                    Points = 1522,
                    Rank = 79
                }
                // new Player
                // {
                //     Id = 8,
                //     Name = "Stephen Curry",
                //     TeamId = 5,
                //     Points = 23668,
                //     Rank = 6
                // },
                // new Player
                // {
                //     Id = 9,
                //     Name = "Kevin Durant",
                //     TeamId = 5,
                //     Points = 28924,
                //     Rank = 5
                // }

            );
            
        }
    }
}