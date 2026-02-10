using Microsoft.EntityFrameworkCore;
using TrickyTrayAPI.Data;
using TrickyTrayAPI.Models;

namespace TrickyTrayAPI.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly TrickyTrayDbContext _context;

        public DonorRepository(TrickyTrayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Donor>> GetAllDonorsWithGiftsAsync()
        {
            // מחזיר את כל התורמים מהטבלה, וטוען לכל אחד מהם את רשימת המתנות שלו
            return await _context.Donors
                .Include(d => d.Gifts)
                .ToListAsync();
        }

        public async Task<Donor?> GetByIdAsync(int id)
        {
            return await _context.Donors
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Donor> CreateAsync(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
            return donor;
        }

        public async Task<Donor?> UpdateAsync(Donor donor)
        {
            var existing = await _context.Donors.FindAsync(donor.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(donor);


            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor == null) return false;

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Donor>> SearchDonorsAsync(string? name, string? email, string? giftName)
        {
            var query = _context.Donors
                .Include(d => d.Gifts)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(d => d.Email.Contains(email));

            if (!string.IsNullOrEmpty(giftName))
                query = query.Where(d => d.Gifts.Any(g => g.Name.Contains(giftName)));

            return await query.ToListAsync();
        }

    }
}
