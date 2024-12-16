using Microsoft.EntityFrameworkCore;
using REST.Models.Classes;

namespace REST.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<User>? UserList { get; set; }
        public DbSet<GameHistory>? GameHistory { get; set; }
        public DbSet<TeamHistory>? TeamHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<TeamHistory>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<GameHistory>().Property(g => g.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<GameHistory>().Property(g => g.Winner).HasMaxLength(100);

            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = 1,
                    Name = "Group 1"
                },
                new Organization
                {
                    Id = 2,
                    Name = "Group 2"
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "test1",
                    // parool on test1
                    Password = "St9tpNN2zrinRGNUgKWCy4JjZRFEorSQ0Zg3a/8m7k4=",
                    OrganizationId = 1,
                },
                new User
                {
                    Id = 2,
                    Username = "test2",
                    Password = "zWoe4T9h2Hj9G4dyUtWwcKwV6zMR1Q0yr3Uch+xSze8=", // test2
                    OrganizationId = 2,
                }
            );

            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Name = "Bulls vs Lakers",
                    OrganizationId = 1
                },
                new Game
                {
                    Id = 2,
                    Name = "Celtics vs Spurs",
                    OrganizationId = 1
                }
            );

            modelBuilder.Entity<GameHistory>().HasData(
                new GameHistory
                {
                    Id = 1,
                    Name = "Võrkpall",
                    OrganizationId = 1,
                    Teams = "Tiim 3 vs Tiim 4",
                    Winner = "Tiim 1"
                },
                new GameHistory
                {
                    Id = 2,
                    Name = "Jalgpall",
                    OrganizationId = 1,
                    Teams = "Tiim 1 vs Tiim 2",
                    Winner = "Tiim 1"
                }                
            );

            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    Id = 1,
                    Name = "Bulls",
                    GameId = 1,
                    OrganizationId = 1
                },
                new Team
                {
                    Id = 2,
                    Name = "Lakers",
                    GameId = 1,
                    OrganizationId = 1
                },
                new Team
                {
                    Id = 3,
                    Name = "Celtics",
                    GameId = 2,
                    OrganizationId = 1
                },
                new Team
                {
                    Id = 4,
                    Name = "Spurs",
                    GameId = 2,
                    OrganizationId = 1
                },
                new Team
                {
                    Id = 5,
                    Name = "IT",
                    OrganizationId = 2
                },
                new Team
                {
                    Id = 6,
                    Name = "Kiired ja vihased",
                    OrganizationId = 2
                }
            );
            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    Id = 1,
                    Name = "Michael Jordan",
                    TeamId = 1,
                    Points = 32292,
                    OrganizationId = 1
                },
                new Player
                {
                    Id = 2,
                    Name = "LeBron James",
                    TeamId = 2,
                    Points = 35367,
                    OrganizationId = 1
                },
                new Player
                {
                    Id = 3,
                    Name = "Kobe Bryant",
                    TeamId = 2,
                    Points = 33643,
                    OrganizationId = 1
                },
                new Player
                {
                    Id = 4,
                    Name = "Magic Johnson",
                    TeamId = 2,
                    Points = 17707,
                    OrganizationId = 1
                },
                new Player
                {
                    Id = 5,
                    Name = "Larry Bird",
                    TeamId = 3,
                    Points = 21791,
                    OrganizationId = 1
                },
                new Player
                {
                    Id = 6,
                    Name = "Kawhi Leonard",
                    TeamId = 4,
                    Points = 13937,
                    OrganizationId = 1
                },
                new Player
                {
                    Id = 7,
                    Name = "Victor Wembanyama",
                    TeamId = 4,
                    Points = 1522,
                    OrganizationId = 1
                }
            );

        }
    }
}