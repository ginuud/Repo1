using api.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Player> Players{ get; set; }
        public DbSet<Game> Games{ get; set; }
        public DbSet<Team> Teams{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Player>()
        .HasOne(p => p.Team)       // Each player has one team
        .WithMany(t => t.Players)  // Each team has many players
        .HasForeignKey(p => p.TeamId);
            modelBuilder.Entity<Player>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Team>().HasData(
                new Team {
                    Id = 1,
                    Name = "Team 1"
                },
                new Team {
                    Id = 2,
                    Name = "Team 2"
                }
            );

                        modelBuilder.Entity<Game>().HasData(
                new Game {
                    Id = 1,
                    Name = "Game 1"
                }
            );
            
            modelBuilder.Entity<Player>().HasData(
                new Player {
                    Id = 1,
                    Name = "Mari",
                    Points = 0,
                    GameId = 1,
                    TeamId = 1 
                },
                new Player {
                    Id = 2,
                    Name = "Margus",
                    Points = 0,
                    GameId = 1,
                    TeamId = 1
                },
                new Player {
                    Id = 3,
                    Name = "Mati",
                    Points = 0,
                    GameId = 1,
                    TeamId = 1
                },
                new Player {
                    Id = 4,
                    Name = "Aadu",
                    Points = 0,
                    GameId = 1,
                    TeamId = 2
                },
                new Player {
                    Id = 5,
                    Name = "Sass",
                    Points = 0,
                    GameId = 1,
                    TeamId = 2
                },
                new Player {
                    Id = 6,
                    Name = "Priit",
                    Points = 0,
                    GameId = 1,
                    TeamId = 2
                }
            );
        }
    }
}