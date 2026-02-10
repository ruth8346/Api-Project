using Microsoft.EntityFrameworkCore;
using TrickyTrayAPI.Data;
using TrickyTrayAPI.Models;

namespace TrickyTrayAPI.Repositories
{
    public class GiftRepository : IGiftRepository
    {
        private readonly TrickyTrayDbContext _context;

        public GiftRepository(TrickyTrayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gift>> GetAllAsync()
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .ToListAsync();
        }

        public async Task<Gift?> GetByIdAsync(int id)
        {
            return await _context.Gifts
                .AsNoTracking()
                .Include(p => p.Donor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Gift> CreateAsync(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            return gift;
        }

        public async Task<Gift?> UpdateAsync(Gift gift)
        {
            var existing = await _context.Gifts.FindAsync(gift.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(gift);


            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null) return false;

            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Gift>> SearchGiftsAsync(string? name, string? donorName, int? buyersCount)
        {
            var query = _context.Gifts
                .Include(g => g.Donor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(g => g.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(donorName))
                query = query.Where(g => g.Donor.Name.Contains(donorName));
            if (buyersCount.HasValue)
            {
                query = query.Where(g =>
                    _context.OrderGift
                        .Where(og => og.GiftId == g.Id)
                        .Select(og => og.Order.BuyerId)
                        .Distinct()
                        .Count() == buyersCount.Value
                );
            }

            return await query.ToListAsync();
        }

        public async Task<List<Gift>> GetSortedGiftsAsync(string sortBy)
        {
            var query = _context.Gifts
                .AsQueryable();

            if (sortBy == "price")
                query = query.OrderByDescending(g => g.Price);
            else if (sortBy == "category")
                query = query.OrderBy(g => g.Category);

            return await query.ToListAsync();
        }

    }
}