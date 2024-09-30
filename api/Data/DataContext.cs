using api.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Player> Players{ get; set; }
        public DbSet<Game> Games{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Player>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Player>().HasData(
                new Player {
                    Id = 1,
                    Name = "Mari",
                    Points = 0,
                    GameId = 1,
                    Game = new Game { Id = 1, Name= "Game 1" },
                    Team = new Team { Id = 1, Name= "Team 1" }
                },
                new Player {
                    Id = 2,
                    Name = "Margus",
                    Points = 0,
                    GameId = 1,
                    Game = new Game { Id = 1, Name= "Game 1"},
                    Team = new Team { Id = 1, Name= "Team 1" }
                },
                new Player {
                    Id = 3,
                    Name = "Mati",
                    Points = 0,
                    GameId = 1,
                    Game = new Game { Id = 1, Name= "Game 1" },
                    Team = new Team { Id = 1, Name= "Team 1" }
                },
                new Player {
                    Id = 4,
                    Name = "Aadu",
                    Points = 0,
                    GameId = 1,
                    Game = new Game { Id = 1, Name= "Game 1" },
                    Team = new Team { Id = 1, Name= "Team 1" }
                },
                new Player {
                    Id = 5,
                    Name = "Sass",
                    Points = 0,
                    GameId = 1,
                    Game = new Game { Id = 1, Name= "Game 1" },
                    Team = new Team { Id = 1, Name= "Team 1" }
                },
                new Player {
                    Id = 6,
                    Name = "Priit",
                    Points = 0,
                    GameId = 1,
                    Game = new Game { Id = 1, Name= "Game 1" },
                    Team = new Team { Id = 1, Name= "Team 1" }
                }
            );
        }
    }
}