using TrickyTrayAPI.Data;
using TrickyTrayAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TrickyTrayAPI.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly TrickyTrayDbContext _context;

        public BuyerRepository(TrickyTrayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Buyer>> GetAllAsync()
        {
            return await _context.Buyers
                .ToListAsync();
        }

        public async Task<Buyer?> GetByIdAsync(int id)
        {
            return await _context.Buyers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Buyer?> GetByEmailAsync(string email)
        {
            return await _context.Buyers
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Email == email);
        }

        public async Task<Buyer> CreateAsync(Buyer buyer)
        {
            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync();
            return buyer;
        }

        public async Task<Buyer?> UpdateAsync(Buyer buyer)
        {
            var existing = await _context.Buyers.FindAsync(buyer.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(buyer);


            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var buyer = await _context.Buyers.FindAsync(id);
            if (buyer == null) return false;

            _context.Buyers.Remove(buyer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
