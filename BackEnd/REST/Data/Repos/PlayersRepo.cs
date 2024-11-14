using Microsoft.EntityFrameworkCore;
using REST.Dtos.Player;
using REST.Interfaces;
using REST.Models.Classes;

namespace REST.Data.Repos
{
    public class PlayersRepo : IPlayerRepository
    {
        private readonly DataContext _context;
        public PlayersRepo( DataContext context )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Player> CreateAsync(Player player){
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
            
            return player;
        }

        public async Task<List<Player>> GetAllAsync()
        {
            var players = _context.Players.Include(t => t.Team).AsQueryable();

            return await players.ToListAsync();
        }
        public async Task<Player?> GetByIdAsync(int id) => await _context.Players
        .Include(t => t.Team)
        .ThenInclude(m => m.Members)
        .FirstOrDefaultAsync(i => i.Id == id);
        
        public async Task<bool> PlayerExists(int id) => await _context.Players.AnyAsync(p => p.Id == id);

        public async Task<Player?> UpdateAsync(int id, UpdatePlayerRequestDto playerDto) 
        {
            var existingPlayer = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);

            if (existingPlayer == null) {
                return null;
            }
            existingPlayer.Name = playerDto.Name;
            existingPlayer.Points = playerDto.Points;
            existingPlayer.TeamId = playerDto.TeamId;

            await _context.SaveChangesAsync();

            return existingPlayer;
        }

        public async Task<Player?> DeleteAsync(int id) 
        {
            var playerModel = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);

            if (playerModel == null) {
                return null;
            }

            _context.Players.Remove(playerModel);
            await _context.SaveChangesAsync();
            return playerModel;	
        }
    }
}
// namespace REST.Data.Repos
// {
//     public class PlayersRepo : IPlayerRepository
//     {
//         // Private readonly field to store the DataContext
//         private readonly DataContext _context;

//         // Constructor that takes the DataContext and assigns it to the private field
//         public PlayersRepo(DataContext context)
//         {
//             _context = context; // Assigning the injected context to the private _context field
//         }

//         public async Task<Player> CreateAsync(Player player)
//         {
//             await _context.Players.AddAsync(player); // Use _context instead of context
//             await _context.SaveChangesAsync();
            
//             return player;
//         }

//         public async Task<List<Player>> GetAllAsync()
//         {
//             var players = _context.Players.Include(t => t.Team).AsQueryable(); // Use _context
//             return await players.ToListAsync();
//         }

//         public async Task<Player?> GetByIdAsync(int id) =>
//             await _context.Players
//                 .Include(t => t.Team)
//                 .ThenInclude(m => m.Members)
//                 .FirstOrDefaultAsync(i => i.Id == id); // Use _context

//         public async Task<bool> PlayerExists(int id) =>
//             await _context.Players.AnyAsync(p => p.Id == id); // Use _context

//         public async Task<Player?> UpdateAsync(int id, UpdatePlayerRequestDto playerDto)
//         {
//             var existingPlayer = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);

//             if (existingPlayer == null)
//             {
//                 return null;
//             }
            
//             existingPlayer.Name = playerDto.Name;
//             existingPlayer.Points = playerDto.Points;
//             existingPlayer.TeamId = playerDto.TeamId;

//             await _context.SaveChangesAsync();

//             return existingPlayer;
//         }

//         public async Task<Player?> DeleteAsync(int id)
//         {
//             var playerModel = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);

//             if (playerModel == null)
//             {
//                 return null;
//             }

//             _context.Players.Remove(playerModel);
//             await _context.SaveChangesAsync();

//             return playerModel;
//         }
//     }
// }
